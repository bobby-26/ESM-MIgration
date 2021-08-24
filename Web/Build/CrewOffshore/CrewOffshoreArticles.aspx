<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreArticles.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreArticles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Articles</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
          <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvCrewArticelSearch.ClientID %>"));
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
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
      
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


                <eluc:TabStrip ID="Crewartical" runat="server" OnTabStripCommand="CrewarticelMenu_TabStripCommand"></eluc:TabStrip>

         
                    <%--  <asp:GridView ID="gvCrewArticelSearch" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewArticel_RowDataBound"
                    OnRowCommand="gvCrewArticel_RowCommand" ShowHeader="true" EnableViewState="false"
                    AllowSorting="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>

                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewArticelSearch" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true" GroupingEnabled="false" OnNeedDataSource="gvCrewArticelSearch_NeedDataSource"
                        
                        OnItemCommand="gvCrewArticelSearch_ItemCommand"
                        OnItemDataBound="gvCrewArticelSearch_ItemDataBound"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" >
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                            <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Updated Date" UniqueName="FLDUPDATEON" SortExpression="FLDUPDATEON" AllowSorting="true" DataField="FLDUPDATEON">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblarticle" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDARTICLEID"]%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lbllastupdatedate" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDUPDATEON"])%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Update Done" AllowFiltering="false">

                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDUPDATEDONE"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Update By" AllowFiltering="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblupdatedby" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDUPDATEDBY"]%>'></telerik:RadLabel>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDUPDATEDBY").ToString() == "" ? "" : "<b>On</b>"%>
                                        <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDMODIFIEDDATE", "{0:dd/MMM/yyyy}"))%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks" DataField="FLDREMARKS" UniqueName="FLDREMARKS"
                                    HeaderStyle-Width="200px" SortExpression="FLDREMARKS">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemarks" runat="server" Width="300px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>50 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                        <eluc:ToolTip ID="ucToolTipRemarks" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREMARKS") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                               
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>

                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Attachment" 
                                            CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Vessel list"
                                            CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="imgaddedvessel"
                                            ToolTip="Vessel list">
                                            <span class="icon"><i class="fas fa-list-alt-annexure"></i></span>
                                        </asp:LinkButton>
                                       
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                           <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

             
            </telerik:RadAjaxPanel>
    

    </form>
</body>
</html>
