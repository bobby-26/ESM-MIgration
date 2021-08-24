<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderFormComments.aspx.cs"
    Inherits="PurchaseOrderFormComments" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Comments</title>
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuMainFilter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuMainFilter" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuSaveFilter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuSaveFilter" />
                        <telerik:AjaxUpdatedControl ControlID="rgvPurchaseRemarks" UpdatePanelHeight="75%" />
                        <telerik:AjaxUpdatedControl ControlID="txtNotesDescription" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvPurchaseRemarks">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvPurchaseRemarks" UpdatePanelHeight="75%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="Div2" style="margin-left: 0px; vertical-align: top;
                width: 100%">
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuMainFilter" runat="server" OnTabStripCommand="MenuMainFilter_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                
                <eluc:TabStrip ID="MenuSaveFilter" runat="server" OnTabStripCommand="MenuSaveFilter_TabStripCommand"
                    TabStrip="false"></eluc:TabStrip>
                </div>
            <br clear="all" />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td style="width:100%;align-items:center;">
                            <telerik:RadTextBox ID="txtNotesDescription" runat="server" RenderMode="Lightweight" Width="80%" TextMode="MultiLine" Rows="3"
                                    EmptyMessage="Enter Your Comments here"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            

            <br clear="all" />
                <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="rgvPurchaseRemarks" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvPurchaseRemarks_NeedDataSource" OnPreRender="rgvPurchaseRemarks_PreRender">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Posted By" UniqueName="POSTEDBY">
                                <ItemStyle Width="250px" />
                                <HeaderStyle Width="250px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Comments" UniqueName="COMMENTS">
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblComments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Posted Date" UniqueName="POSTEDDATE">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPostedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" Width="100%" VerticalAlign="Middle" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                    </ClientSettings>
                </telerik:RadGrid>
                </div>
    </form>
</body>
</html>
