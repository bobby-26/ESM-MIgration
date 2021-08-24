<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMISPrincipalwiseReturneeRate.aspx.cs"
    Inherits="CrewReportMISPrincipalwiseReturneeRate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EmpFleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrew1.ClientID %>"));
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
           
                        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
               
             <table width="100%" cellpadding="1" cellspacing="1">
                    <tr style="color: black">
                        <td colspan="6" style="padding-left:10px">
                       <font color="blue"> Note :&nbsp;To view the Guidelines, put the mouse on the&nbsp;&nbsp;
                            <asp:LinkButton id="imgnotes" runat="server" >
                                <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                </asp:LinkButton>
                            &nbsp; button.</font>
                            <eluc:ToolTip ID="ucToolTipNW" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:10px">
                            <telerik:radlabel ID="lblNationality" runat="server" Text="Nationality"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true"  Width="240px"/>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblZone" runat="server" Text="Zone"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="240px"/>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblEmpFleet" runat="server" Text="Emp Fleet"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:EmpFleet ID="ucFleet" AppendDataBoundItems="true" runat="server" Width="210px"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:10px">
                            <telerik:radlabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="240px"/>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblPool" runat="server" Text="Pool"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true"  Width="240px"/>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblSelectManagerPrincipal" runat="server" Text="Select Manager/Principal"></telerik:radlabel>
                            <br />
                            <br />
                            <br />
                            <br />
                            <telerik:radlabel ID="lblManagerPrincipal" runat="server" Text="Manager/Principal"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblPrincipalManager" runat="server" RepeatDirection="Horizontal" 
                                AutoPostBack="true" OnSelectedIndexChanged="PrincipalManagerClick">
                                <Items>
                                <telerik:ButtonListItem Value="1" Selected="true"   Text="Manager" />
                                <telerik:ButtonListItem Value="2"   Text="Principal" />
                                    </Items>
                            </telerik:RadRadioButtonList>
                            <br />
                            <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true"   Width="210px"     CssClass="dropdown_mandatory" />
                            <eluc:Principal ID="ucPrinicpal" runat="server" AddressType="128" CssClass="dropdown_mandatory" Width="210px"  AppendDataBoundItems="true" Visible="false" />
                        </td>
                        
                    </tr>
                    <tr>
                       <td style="padding-left:10px">
                            <telerik:radlabel ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true"  Width="240px"/>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblBatch" runat="server" Text="Batch"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" Width="240px"/>
                        </td>
                        <td colspan="2" rowspan="2">
                            <asp:Panel ID="pnlDate" runat="server" GroupingText="Period" Width="85%">
                                <telerik:radlabel ID="lblFrom" runat="Server" Text="From "></telerik:radlabel>
                                <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                                <telerik:radlabel ID="lblTo" runat="Server" Text="To"></telerik:radlabel>
                                <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                
                          <telerik:RadGrid ID="gvCrew1" runat="server" AutoGenerateColumns="False" Font-Size="11px"  AllowPaging="true" AllowCustomPaging="true"
                 CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew1_ItemCommand"
                OnItemDataBound="gvCrew1_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew1_NeedDataSource" RenderMode="Lightweight" AllowSorting="true" >
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
                                    <telerik:radlabel ID="lblRankName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="All Joiners">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblrankid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                                    <telerik:radlabel ID="lblAllJoiners" Visible="false" runat="server" Text="3" />
                                   
                                        <asp:LinkButton ID="lnkAllJoiners" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLJOINERS") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="New Joiners">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblrankid1" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                                    <telerik:radlabel ID="lblNewJoiners" Visible="false" runat="server" Text="1" />
                                
                                        <asp:LinkButton ID="lnkNewJoiners" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEW") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Re Joiners">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblrankid2" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                                    <telerik:radlabel ID="lblReJoiners" Visible="false" runat="server" Text="2" />
                                
                                        <asp:LinkButton ID="lnkReJoiners" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJOINER") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Returnee Rate">
                                
                                <ItemTemplate>
                                   
                                        <telerik:radlabel ID="lblReturneeRate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRETURNEERATE") %>' />
                                   
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