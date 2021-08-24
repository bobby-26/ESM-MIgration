<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDrillOfficeDashboard.aspx.cs"
    Inherits="Registers_DrillOfficeDashboard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drill Office Dashboard</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDrillofiicedashboard.ClientID %>"));
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
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
        DecorationZoneID="gvDrillofiicedashboard" DecoratedControls="All" EnableRoundedCorners="true" />
     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:TabStrip ID="Tabstripdrillofficedashboardmenu" runat="server" OnTabStripCommand="drilljobregistermenu_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>
    <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvDrillofiicedashboard" AutoGenerateColumns="false"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvDrillofiicedashboard_NeedDataSource" 
            OnItemDataBound="gvDrillofiicedashboard_ItemDataBound" ShowFooter="false">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDDRILLID" AutoGenerateColumns="false" EnableColumnsViewState ="false"
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" >
                <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true" ></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                <Columns>
                <telerik:GridTemplateColumn HeaderText="Mandatory Drills">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblDrillName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLNAME")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                        
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Over Due">
                        <HeaderStyle HorizontalAlign="Center" BackColor="Red" Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Center" />
                         <FooterStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a id="overdueanchor" runat="server"  style="text-decoration:none;color:black"><%# DataBinder.Eval(Container, "DataItem.FLDOVERDUE")%></a>
                        </ItemTemplate>
                        <FooterTemplate>
                        <a id="overduecsanchor" runat="server"  style="text-decoration:none;color:black">
                        <telerik:RadLabel ID="Radlblcompanyspecifiedoverdue" runat="server" >
                            </telerik:RadLabel></a>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
