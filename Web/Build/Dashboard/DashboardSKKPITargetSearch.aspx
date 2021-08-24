<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKKPITargetSearch.aspx.cs" Inherits="Dashboard_DashboardSKKPITargetSearch" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Key Performance Indicators (KPI) Targets</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
             <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvKPITargetlist.ClientID %>"));
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
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
     
       
      
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
             <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <telerik:RadNotification ID="ucNotification" RenderMode="Lightweight" runat="server" AutoCloseDelay="1500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false"></telerik:RadNotification>
            <eluc:TabStrip ID="Tabkpi" runat="server" OnTabStripCommand="KPI_TabStripMenuCommand" TabStrip="true"></eluc:TabStrip>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

            <table>
                <tr>
                    <th>
                        <telerik:RadLabel ID="Radlblyear" runat="server" Text="Year " />
                    </th>
                    <th>&nbsp
                    </th>
                    <td>
                        <eluc:Year ID="radcbyear" runat= "server" AutoPostBack="true" YearStartFrom="2018"  NoofYearFromCurrent="3" OrderByAsc="True"  OnTextChangedEvent="radcbyear_TextChangedEvent"/>
                    </td>
                    
                </tr>


            </table>
            
            <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="SPI_TabStripMenuCommand" TabStrip="true"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvKPITargetlist" AutoGenerateColumns="false" 
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvKPITargetlist_NeedDataSource" 
                OnItemDataBound="gvKPITargetlist_ItemDataBound"  ShowFooter="false">
                <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" DataKeyNames="FLDKPIID"
                    TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="KPI"   HeaderText="KPI" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"/>

                    </ColumnGroups>

                    <Columns>
                       

                        <telerik:GridTemplateColumn  HeaderText="Code" ColumnGroupName="KPI">
                            <HeaderStyle HorizontalAlign="Left"  Font-Bold="true" Width="90px" />
                            <ItemStyle Width="90px"  HorizontalAlign="Left"/>
 
                            <ItemTemplate>

                                <telerik:RadLabel runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDKPICODE")%> ' />
                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn >

                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="KPI">
                            <HeaderStyle HorizontalAlign="Left"  Font-Bold="true" />
                            <ItemStyle  Wrap="true" HorizontalAlign="Left"/>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" Text='   <%# DataBinder.Eval(Container, "DataItem.FLDKPINAME")%> ' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn  HeaderText="Ref No." ColumnGroupName="KPI">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="200px" />
                            <ItemStyle Width="90px" HorizontalAlign="Center"/>
 
                            <ItemTemplate>

                               
                                 <telerik:RadTextBox runat="server" ID="radtxtreferenceno" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFERENCENO")%> '/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn >
                           <telerik:GridTemplateColumn HeaderText="Minumum " >
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="90px"  />
                           <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                
                                <telerik:RadTextBox runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDKPIMINVALUE")%>' ID="radtxtminimum" CssClass="input_mandatory" width="50px" />
                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target " >
                             <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="90px"  />
                           <ItemStyle HorizontalAlign="Center" />
                           
                            <ItemTemplate>
                               
                                <telerik:RadTextBox runat="server" ID="radtxttarget" CssClass="input_mandatory" Width="50px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDKPITARGETVALUE")%> ' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Objective Owner " >
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="300px"  />
                           
                            <ItemTemplate>
                                  <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="Radcombodesignationlist" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDUSERNAME" DataValueField="FLDUSERCODE" DropDownWidth="300px" 
                            Placeholder="Type to select to whom KPI is to be Assigned" Filter="Contains" FilterFields="FLDDESIGNATIONNAME, FLDUSERNAME"  >
                            <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDDESIGNATIONNAME" Title="Designation" Width="120px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDUSERNAME" Title="Name" Width="150px" />
                            </ColumnsCollection>
                         </telerik:RadMultiColumnComboBox>
                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
