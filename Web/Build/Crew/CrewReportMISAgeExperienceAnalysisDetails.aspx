<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMISAgeExperienceAnalysisDetails.aspx.cs"
    Inherits="Crew_CrewReportMISAgeExperienceAnalysisDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MIS Age Analysis Details</title>
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
  
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
          
                        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                
                  <b>&nbsp&nbsp Note:</b> Select 'Rank', 'Age greater than' or 'Age Range' To View Particular Details
            
                            <table width="100%">
                            <tr>
                                <td style="padding-left:10px;padding-right:15px">
                                    <telerik:radlabel ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                                </td>
                                <td style="padding-right:15px">
                                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="240px" Enabled="false"/>
                                </td>
                                 <td style="padding-right:15px">
                                    <telerik:radlabel ID="lblAgeRange" runat="server" Text="Age Ranges"></telerik:radlabel>
                                </td>
                               <td style="padding-right:15px">
                                    <telerik:RadComboBox ID="ddlAgeRange" runat="server" OnSelectedIndexChanged="ddlSelectRange_SelectedChangeEvent"  Width="180px"  CssClass="dropdown_mandatory"  Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Course">
                                        <Items>                                        
                                        <telerik:RadComboBoxItem text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem text="18-25"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem text="26-30"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem text="31-35"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem text="36-40"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem text="41-45"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem text="46-50"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem text="51-55"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem text="56-60"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem text="Above 60"></telerik:RadComboBoxItem>
                                            </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                   
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                
                         <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"  AllowPaging="true" AllowCustomPaging="true"
                 CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew_ItemCommand"
                OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew_NeedDataSource" RenderMode="Lightweight" AllowSorting="true" >
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
                            <telerik:gridtemplatecolumn headertext="Rank">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblJoinedRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Emp No">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmpFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Name">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                    <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Age">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblAge" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Experience in Rank (Months)">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblExpRankMonths" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKEXP") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Nationality">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Current Vessel">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblCurrentVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVESSEL") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Signed On">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSignedOn" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Last Vessel">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblLastVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSEL") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Signed Off">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSignedOff" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE") %>' />
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

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
