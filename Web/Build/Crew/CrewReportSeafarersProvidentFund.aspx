<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportSeafarersProvidentFund.aspx.cs" Inherits="Crew_CrewReportSeafarersProvidentFund" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselList.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seafarers Provident Fund</title>
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
            
                            <eluc:TabStrip ID="CrewMenuGeneral" runat="server" OnTabStripCommand="CrewMenuGeneral_TabStripCommand">
                        </eluc:TabStrip>
                
                        <table cellpadding="1" cellspacing="1" width="80%" >
                        <tr>
                            <td>
                                <telerik:radlabel ID="lblUnion" runat="server" Text="Union"></telerik:radlabel>
                            </td>
                            <td>
                                <eluc:Address ID="ucUnion" runat="server" AddressType="134"  AppendDataBoundItems="true"  Width="210px"
                                    CssClass="dropdown_mandatory" />
                            </td>
                            
                            <td>
                                <telerik:radlabel ID="lblComponent" runat="server" Text="Component Name"></telerik:radlabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtComponent" runat="server" Text="SPF" ReadOnly="true" CssClass="readonlytextbox">
                                </telerik:RadTextBox>
                            </td>
                            </tr>
                            <tr>
                            <td style="vertical-align: top">
                                <telerik:radlabel ID="lblVessels" runat="server" Text="Vessels"></telerik:radlabel>
                            </td>
                            <td style="vertical-align: top" >                                   
                                <div runat="server" id="dvClass" class="input" style="overflow: auto;  Width:210px; height: 70px">
                                       <telerik:RadCheckBoxList ID="chkVessels" runat="server"  >
                                        <Databindings DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" />          
                                       </telerik:RadCheckBoxList>
                                      
                                </div>
                            </td>
                                
                            <td colspan="2" >
                                <asp:Panel ID="pnlPeriod" runat="server" GroupingText="Period" width="70%">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:radlabel ID="lblFromDate" Text="From Date" runat="server"></telerik:radlabel>
                                                
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucDate" runat="server" width="150px" />
                                            </td>
                                            <td>
                                                <telerik:radlabel ID="lblToDate" runat="server" Text="To Date"></telerik:radlabel>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucDate1" runat="server"  width="150px"  />
                                            </td>
                                         </tr>
                                     </table>
                                </asp:Panel></td>
                        </tr>
                        </table>
                   
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>

                   <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" 
               CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
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
                            <telerik:gridtemplatecolumn headertext="Union Member Name">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblempname" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Union Code">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblcode" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="IMO No">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblimono" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMONUMBER") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Sign in">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="dtSignon" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Sign out">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="dtSignoff" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Passport No">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblPassportno" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Union Member Address">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblAddress" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESS") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Email Address">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmail" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Gender">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblGender" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="DOB">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="dtdob" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Nationality Code">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblNationalitycode" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>' />
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