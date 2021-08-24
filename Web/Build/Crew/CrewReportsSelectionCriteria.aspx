<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsSelectionCriteria.aspx.cs"
    Inherits="CrewReportsSelectionCriteria" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCNTBRReason" Src="~/UserControls/UserControlNTBRReasonList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="panel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
   
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           
                        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
               
                 <table width="100%">
                    <tr>
                        <td>
                            <telerik:radlabel ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true"   Width="240px" />
                        </td>
                        <td rowspan="3">
                            <table cellspacing="10">
                                <tr>
                                    <td style="padding-left:10px">
                                        <telerik:radlabel ID="lblAgeBetween" runat="server" Text="Age Between"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAgeF" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:radlabel ID="lblAgeFand" runat="server" Text="and"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAgeT" runat="server"  />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left:10px">
                                        <telerik:radlabel ID="lblNoofCompaniesChanges" runat="server" Text="No.Of Companies Changed in the &nbsp;&nbsp;&nbsp;&nbsp;</br>Last 5 Years"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCmpyChangedF" runat="server"  />
                                    </td>
                                    <td>
                                        <telerik:radlabel ID="lblCmpyChangedFand" runat="server" Text="and"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCmpyChangedT" runat="server"  />
                                    </td>
                                </tr>
                                <tr>
                                   <td style="padding-left:10px">
                                        <telerik:radlabel ID="lblCombinedRankExperience" runat="server" Text="Combined Rank Experience(3E / 4E)"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtRExp3e4eF" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:radlabel ID="lblRexpand" runat="server" Text="and"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtRExp3e4eT" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left:10px">
                                        <telerik:radlabel ID="lblCombinedRankExperience2" runat="server" Text="Combined Rank Experience(MST/CO)"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtRExpmstcoF" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:radlabel ID="lblRexpmst" runat="server" Text="and"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtRExpmstcoT" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left:10px">
                                        <telerik:radlabel ID="lblCombinedRankExperience1" runat="server" Text="Combined Rank Experience(2O / 3O)"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtRExp2o3oF" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:radlabel ID="lblRexp2" runat="server" Text="and"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtRExp2o3oT" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left:10px">
                                        <telerik:radlabel ID="lblCombinedRankExperience3" runat="server" Text="Combined Rank Experience(CE / 2E)"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtRExpce2eF" runat="server"  />
                                    </td>
                                    <td>
                                        <telerik:radlabel ID="lblRExpce2" runat="server" Text="and"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtRExpce2eT" runat="server"  />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblExhand" runat="server" Text="Exhand"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlExhand" runat="server"   Width="240px"  Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Exhand">
                             <Items>
                                <telerik:RadComboBoxItem Value="" Text="All"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="1" Text="Yes"/>
                                <telerik:RadComboBoxItem Value="0" Text="No" />
                                 </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:radlabel ID="lblPool" runat="server" Text="Pool"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true"  Width="240px" />
                        </td>
                        <td>
                            <telerik:radlabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:Principal ID="ucPrinicipal" runat="server" AddressType="128" AppendDataBoundItems="true"  Width="240px"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:radlabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:radlabel>
                        </td>
                        <td>
                            <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true"   Width="240px" />
                        </td>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:radlabel ID="lblSignOn" runat="server" Text="Sign On Date"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucSignOnFrom" runat="server" Width="115px" />
                                    </td>
                                    <td>
                                        <telerik:radlabel runat="server" Text="and" ID="lblSignOnand"></telerik:radlabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucSignOnTo" runat="server"  Width="115px"   />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:radlabel ID="lblRatingExperience" runat="server" Text="Experience as Rating"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlRatingExp" runat="server"  Width="240px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Experience">
                                <Items>
                               <telerik:RadComboBoxItem Text="--Select--" Value="" />
                               <telerik:RadComboBoxItem Text="Yes" Value="1"       />
                               <telerik:RadComboBoxItem Text="No" Value="0"        />
                                    </Items>
                                    </telerik:RadComboBox>                         
                       </td>
                   </tr>
                </table>
               
                 <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
             
            <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="false"  AllowCustomPaging="true" AllowPaging="true"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="true" OnItemCommand="gvCrew_ItemCommand" 
                OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew_NeedDataSource" AllowSorting="false" AllowFilteringByColumn="false" EnableLinqExpressions="false">                
                <MasterTableView AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true">
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

                           <%--<telerik:gridtemplatecolumn headertext="S.No.">
                                                               
                               <ItemTemplate>
                                  <%# DataBinder.Eval(Container,"DataItem.ItemIndex + 1") %>
                               </ItemTemplate>
                            
                            </telerik:gridtemplatecolumn>--%>
                            <telerik:GridTemplateColumn HeaderText="Employee Code">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmpNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") + "- " + DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Batch">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNO") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmpId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                    <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKPOSTEDNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel Onboard">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblPVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSELNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="SignOn Date">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSignOnDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRESENTSIGNONDATE", "{0:dd/MMM/yyyy}")%>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Exhand (Y/N)">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblExhand" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXHANDYN") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Full Contract (Y/N)">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblFullContract" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLCONTRACTYN") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Age">
                             
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblAge" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="No. of Companies">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblCompanies" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYCHANGED") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Combined Rank Experience">
                               
                                 <ItemTemplate>
                                    <telerik:radlabel ID="lblExperience" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMBINEDEXP") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                          </Columns>
                     <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>