<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRRequisitionandPODetails.aspx.cs" Inherits="CrewOffshoreDMRRequisitionandPODetails" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ReQuisition and PO Details</title>
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
         <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
                </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
               
              
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                
                <div id="divGrid" runat="server" style="position: relative; overflow: auto; z-index: 0">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvRequisitionandPO" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    OnItemDataBound="gvRequisitionandPO_ItemDataBound"
                    OnNeedDataSource="gvRequisitionandPO_NeedDataSource"
                    OnItemCommand ="gvRequisitionandPO_ItemCommand"
                        OnPageIndexChanged="gvRequisitionandPO_PageIndexChanged">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDPONUMBER" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblSlNoHeader" runat="server">Sl.No.</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSlno" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                     <telerik:RadLabel ID="lblRankHeader" runat="server">PO Number</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPONo"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPONUMBER") %>' CommandArgument="<%# Container.DataSetIndex %>" 
                                        CommandName="ORDER"></asp:LinkButton>
                                    <telerik:RadLabel ID="lblOrderid"   Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                               <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblSignOffDateHeader" runat="server">Date</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <telerik:RadLabel ID="lblDate"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblTypeHeader" runat="server">Type</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTypeName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblStatusHeader" runat="server">Status</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                           
                         
                      
                         </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>
             
            </div>
   </telerik:RadAjaxPanel>
    </form>
</body>
</html>
