<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportAddressLabel.aspx.cs"
    Inherits="CrewReportAddressLabel" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seafarers OnBoard OnLeave Report</title>
   <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
            
                        <table border="1" style="border-collapse: collapse;" width="100%">
                            <tr valign="top">
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblManager" Text="Manager" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                                                     />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblPrincipal" Text="Principal" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Principal ID="ucPrinicipal" runat="server" AddressType="128" AppendDataBoundItems="true" 
                                                     />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr valign="top">
                                            <td colspan="2">
                                                <telerik:RadLabel ID="lblVesselType" Text="Vessel Type" runat="server"></telerik:RadLabel>
                                               
                                                    <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true"  />
                                          
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblPool" Text="Pool" runat="server"></telerik:RadLabel>
                                            
                                                    <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="220px" />
                                        
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblZone" Text="Zone" runat="server"></telerik:RadLabel>
                                             
                                                    <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="220px"/>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblRank" Text="Rank" runat="server"></telerik:RadLabel>
                                                
                                                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true"  />
                                               
                                            </td>
                                        </tr>
                                    </table>
                    
                    </td> </tr> </table>
       
                <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                </eluc:TabStrip>

                 <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="68%"
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
            EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound" OnItemCommand="gvCrew_ItemCommand"
            ShowFooter="False" OnSortCommand="gvCrew_SortCommand" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" >
                <HeaderStyle Width="102px" />

                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                    Font-Bold="true">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="48px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         
                           
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEECODE")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>

                           <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="320px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>   
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDNAME")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                         <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKPOSTEDNAME")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                        
                           <telerik:GridTemplateColumn HeaderText="City" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                          <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCITY")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="State" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                           <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSTATE")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Pin Code" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>  
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPOSTALCODE")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                    </Columns>
                 <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
