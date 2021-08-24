<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportReasonwiseIllnessAndInjuryDetails.aspx.cs" Inherits="Crew_CrewReportReasonwiseIllnessAndInjuryDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reason Wise Illness/Injury Details</title>
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
            
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                

                        <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" OnSorting="gvCrew_Sorting"
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
                               <telerik:gridtemplatecolumn headertext="Seafarer Name" allowsorting="true" sortexpression="FLDNAME">
                                
                                   <ItemTemplate>
                                    <telerik:radlabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                    <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            
                             <telerik:gridtemplatecolumn headertext="Rank">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            
                            <telerik:gridtemplatecolumn headertext="Date Of Injury/Illness">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblDateOfInjury" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFILLNESS") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            
                               <telerik:gridtemplatecolumn headertext="Reason">
                                
                                   <ItemTemplate>
                                    <telerik:radlabel ID="lblReason" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPESOFINJURY") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            
                             <telerik:gridtemplatecolumn headertext="Case No">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblCaseNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNIMEDICALCASEID") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Port">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblPort" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORT") %>' />
                                </ItemTemplate>
                                <ItemStyle Wrap="true" />
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Vessel">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblVesselName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Zone">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblZone" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Pool">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblPool" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOOL") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Case Description">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblCaseDesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDILLNESSDESCRIPTIONTOOLTIP") %>' />
                                    <eluc:Tooltip ID="uclblSubject" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDILLNESSDESCRIPTION").ToString() %>' />
                                </ItemTemplate>
                                <ItemStyle Wrap="true" />
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