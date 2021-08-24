<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOfficeLeaveWagesAndSideLetter.aspx.cs"
    Inherits="AccountsOfficeLeaveWagesAndSideLetter" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Src="~/UserControls/UserControlVessel.ascx" TagName="Vessel" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="Status" TagPrefix="eluc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew BOW</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="form1" runat="server">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
    
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
            
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:TabStrip ID="MenuPB" runat="server" OnTabStripCommand="MenuPB_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                            
                <table cellpadding="1" cellspacing="1">                   
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" Width="190px" VesselsOnly="true" AutoPostBack="true"
                                CssClass="input" />
                        </td>
                    </tr>
                </table>
          

              <telerik:RadGrid RenderMode="Lightweight" ID="gvPB" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvPB_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" Height="93%" 
                ShowFooter="false" ShowHeader="true" OnItemCommand="gvPB_ItemCommand" OnItemDataBound="gvPB_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false"  TableLayout="Fixed" CommandItemDisplay="Top">
                     <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                      <Columns>           
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Start Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPbId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDPORTAGEBILLID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELID"] %>'></telerik:RadLabel>                               
                                <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDSTARTDATE"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="End Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDENDDATE"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Voucher Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDVOUCHERNUMBER"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                     <%--   <telerik:GridTemplateColumn HeaderText="Posted Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDVOUCHERDATE"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>   --%>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>                               
                                <telerik:RadLabel ID="lblAction" runat="server" Text="Action"></telerik:RadLabel>                                
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                              
                                 <asp:LinkButton runat="server" AlternateText="PortageBill Posting Draft View" CommandName="DRAFT" ID="cmdDraft" ToolTip="Leave Wages and Side Letter">
                                    <span class="icon"><i class="fas fa-file-alt"></i></span>
                                </asp:LinkButton>
                             <%--   <asp:ImageButton runat="server" AlternateText="PortageBill Posting Draft View" ImageUrl="<%$ PhoenixTheme:images/document_view.png %>"
                                    CommandName="DRAFT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDraft"
                                    ToolTip="Leave Wages and Side Letter"></asp:ImageButton>         --%>                     
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                       <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            </telerik:RadAjaxPanel>
              
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
      </form>
</body>
</html>

           
        
   