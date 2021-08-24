<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportEmployeeGrowth.aspx.cs"
    Inherits="CrewReportEmployeeGrowth" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="../UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="../UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EmpFleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RankList" Src="../UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Growth Report</title>
   <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvEmployeeStatistics.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
            </script>
        <style type="text/css">
            .mlabel {
                color: blue !important;                
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmEmployeeGrowth" runat="server" submitdisabledcontrols="true">
   
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                        <eluc:TabStrip ID="MenuEmployeeGrowthReport" runat="server" OnTabStripCommand="MenuEmployeeGrowthReport_TabStripCommand">
                    </eluc:TabStrip>
                
            <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td colspan="4" style="padding-left:10px">
                            <telerik:radlabel ID="lblguidanceText" runat="server" CssClass="mlabel"> Note: Please click on the no. of seafarers to see the list of seafarers 
                            
                            </telerik:radlabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:10px">
                            <telerik:radlabel ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:RankList ID="lstRank" runat="server"  AppendDataBoundItems="true" Width="150px" />
                            <br />                            
                        </td>
                        <td colspan="2" style="padding-right:35px">
                            <asp:Panel ID="pnlRecruitedperiod" runat="server" GroupingText="Recruited during the period">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:radlabel ID="lblFromDate" Text="From Date" runat="server"></telerik:radlabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory"  Width="120px" />
                                        </td>
                                        </tr>
                                    <tr>
                                        <td>
                                            <telerik:radlabel ID="lblToDate" runat="server" Text="To Date"></telerik:radlabel>
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory"   Width="120px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblBatch" runat="server" Text="Batch"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true"  Width="250px" />
                        </td>
                        <td>
                            <telerik:radlabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128"     Width="150px"        AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:10px">
                            <telerik:radlabel ID="lblZone" runat="server" Text="Zone"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true"  Width="150px" />
                        </td>
                        <td>
                            <telerik:radlabel ID="lblPool" runat="server" Text="Pool"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true"  Width="215px"   />
                        </td>
                        <td>
                            <telerik:radlabel ID="lblEmpFleet" runat="server" Text="Emp Fleet"></telerik:radlabel>
                            
                        </td>
                        <td>
                            <eluc:EmpFleet ID="ucFleet" AppendDataBoundItems="true" runat="server"  Width="250px"  />
                        </td>
                        <td>
                            <telerik:radlabel ID="lblNationality" runat="server" Text="Nationality"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true"  Width="150px"  />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:10px">
                            <telerik:RadRadioButtonList ID="rblGrowth" runat="server" RepeatDirection="Vertical" AutoPostBack="true"
                                OnSelectedIndexChanged="ChangeBind" Visible="false">
                                <Items>
                                <telerik:ButtonListItem Value="3" Text="% Statistics of Seafarers"></telerik:ButtonListItem>
                                    </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                </table>
                
                            <eluc:TabStrip ID="MenuEmployeeStatistics" runat="server" OnTabStripCommand="EmployeeStatistics_TabStripCommand">
                        </eluc:TabStrip>
                    
                                 <telerik:RadGrid ID="gvEmployeeStatistics" runat="server" AutoGenerateColumns="False" Font-Size="11px" ShowFooter="false"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvEmployeeStatistics_ItemCommand"
                OnItemDataBound="gvEmployeeStatistics_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvEmployeeStatistics_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
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

                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                                <telerik:gridtemplatecolumn HeaderText="Total Recruited"> 
                                    
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblTotalActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALACTIVE") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn  HeaderText="OnBoard"    >
                                    
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOnBoard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONBOARDCOUNT") %>' />
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn  HeaderText="OnBoard %"    >
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblOnboardPercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONBOARDPERCENTAGE","{0:n}") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn  HeaderText="OnLeave"    >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOnLeave" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONLEAVECOUNT") %>' />
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn  HeaderText="OnLeave %"    >
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblOnLeavePercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONLEAVEPERCENTAGE","{0:n}") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                               <telerik:gridtemplatecolumn  HeaderText="Active"    >
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn  HeaderText="Active %"    >
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDACTIVEPERCENTAGE","{0:n}") %>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn  HeaderText="InActive"    >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkInActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVE") %>' />
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn  HeaderText="InActive %"    >
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDINACTIVEPERCENTAGE", "{0:n}")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                            </Columns>

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