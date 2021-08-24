<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSundryPurchase.aspx.cs" Inherits="AccountsSundryPurchase" %>


<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SundryPurchase</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

          <script type="text/javascript">

            function PaneResized(sender, args) {
                var splitter = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                splitter.set_height(browserHeight - 60);
                splitter.set_width("100%");
                var grid = $find("gvBondReq");
                var contentPane = splitter.getPaneById("listPane");
                grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 120) + "px";
                //console.log(grid._gridDataDiv.style.height, contentPane._contentElement.offsetHeight);
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmBondReq" runat="server" autocomplete="off">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ifMoreInfo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvBondReq">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvBondReq" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuOrderForm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuOrderForm" />
                        <telerik:AjaxUpdatedControl ControlID="gvBondReq" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucConfirm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvBondReq" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuBondReq">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuBondReq" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>

            
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    
                       
                            <asp:Button ID="cmdHiddenSubmit" CssClass="hidden" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                      
                 
                        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="generalPane" runat="server" Scrolling="None">
             <iframe runat="server" id="ifMoreInfo" style="height: 600px; width: 100%; overflow-x: hidden"></iframe>
                       </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
            <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                        <eluc:TabStrip ID="MenuBondReq" runat="server" OnTabStripCommand="MenuBondReq_TabStripCommand"></eluc:TabStrip>
                     <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvCashOut" DecoratedControls="All" EnableRoundedCorners="true" />

                <telerik:RadGrid RenderMode="Lightweight" ID="gvBondReq" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvBondReq_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvBondReq_SelectedIndexChanging"
                    OnItemDataBound="gvBondReq_ItemDataBound" OnItemCommand="gvBondReq_ItemCommand"
                    ShowFooter="false" ShowHeader="true" OnSortCommand="gvBondReq_SortCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDORDERID">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Order No" AllowSorting="true" SortExpression="FLDREFERENCENO">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                         
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDORDERID"] %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="select"><%# ((DataRowView)Container.DataItem)["FLDREFERENCENO"] %></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Order Date" AllowSorting="true" SortExpression="FLDORDERDATE">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                    <ItemTemplate>
                                        <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDORDERDATE"]) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Stock Type" AllowSorting="true" SortExpression="FLDSTOCKTYPE">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDSTOCKTYPE"] %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn  HeaderText="Received Date" AllowSorting="true" SortExpression="FLDRECEIVEDDATE">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                    <ItemTemplate>
                                        <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDRECEIVEDDATE"])%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn  HeaderText="Order Status" AllowSorting="true" SortExpression="FLDORDERSTATUS">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                    <ItemTemplate>
                              
                                        <%# ((DataRowView)Container.DataItem)["FLDORDERSTATUS"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn  HeaderText="Action" >
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                 
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Confirm" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove"
                                            ToolTip="Confirm"></asp:ImageButton>
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </telerik:RadPane>
        </telerik:RadSplitter>
                   
               
    </form>
</body>
</html>

                  