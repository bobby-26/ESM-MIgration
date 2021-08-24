<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewQuickReportVaccination.aspx.cs"
    Inherits="Crew_CrewQuickReportVaccination" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
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
           
                        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
               
                        <table cellpadding="0" cellspacing="0" >
                        <tr>
                            
                                        <td style="padding-left:10px;padding-right:15px">
                                            <telerik:radlabel ID="lblFromDate1" runat="server" Text="Vaccination"></telerik:radlabel>
                                        </td>
                                       <td style="padding-right:10px">
                                            <eluc:Date ID="ucSignOnFromDate" runat="server" CssClass="input_mandatory" />
                                            <eluc:Date ID="ucSignOnToDate" runat="server" CssClass="input_mandatory" />
                                        </td>
                                 
                                    <td style="padding-right:10px">
                                            <telerik:radlabel ID="lblname" runat="server" Text="Name"></telerik:radlabel>
                                        </td>
                                       <td style="padding-right:10px">
                                            <telerik:RadTextBox ID="txtname" runat="server" Width="150px"></telerik:RadTextBox>
                                        </td>
                                  
                                    <td style="padding-right:10px">
                                            <telerik:radlabel ID="lblFileno" runat="server" Text="File No."></telerik:radlabel>
                                        </td>
                                              <td style="padding-left:10px;padding-right:15px">
                                          <telerik:RadTextBox ID="txtFileNo" runat="server"  Width="150px"  ></telerik:RadTextBox>
                                        </td>
                                    
                    <td style="padding-right:10px">
                               <telerik:radlabel ID="lblVaccination" runat="server" Text="Vaccination"></telerik:radlabel>
                             </td>                               
                         <td style="padding-right:10px">
                                <telerik:RadListBox ID="lstVaccination" runat="server" AppendDataBoundItems="true"  Height="70px" Width="210px"
                                    SelectionMode="Multiple">
                                    <Items>
                                    <telerik:RadListBoxItem Text="--Select--" Value="dummy"></telerik:RadListBoxItem>
                                    </Items>
                              </telerik:RadListBox>
                            </td>
                        </tr>
                    </table>
                  
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
                                    <telerik:RadLabel  ForeColor="Black"  ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                                      <telerik:GridTemplateColumn HeaderText="Vaccination Name">
                                
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblEmpFileNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                        <telerik:radlabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                        <telerik:radlabel ID="lnkName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                        <telerik:radlabel ID="lblRank" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                                        <telerik:radlabel ID="lblBatch" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                                        <telerik:radlabel ID="lblZONE" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONENAME") %>' />
                                        <telerik:radlabel ID="lblLastVesselname" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSELNAME") %>' />
                                        <telerik:radlabel ID="lblSignOffDate" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTSIGNOFFDATE") %>' />
                                        <telerik:radlabel ID="lblPresentVessel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSELNAME") %>' />
                                        <telerik:radlabel ID="lblSignon" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTSIGNONDATE") %>' />
                                        <telerik:radlabel ID="lblDOA" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOA") %>' />
                                        <telerik:radlabel ID="lbllastcontact" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTCONTACTDATE") %>' />
                                        <telerik:radlabel ID="lblActive" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' />
                                        <telerik:radlabel ID="lblRequirement" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERREQUIREMENT") %>' />
                                        <telerik:radlabel ID="lblVaccinationname" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVACCINATIONNAME") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Date of Issue">
                                   
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblIssueDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Date of Expiry">
                                    
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblExpireDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
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