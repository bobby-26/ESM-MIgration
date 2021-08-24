<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListDeliveryLocation.aspx.cs"
    Inherits="CommonPickListDeliveryLocation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seaport List</title>
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

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
                <telerik:AjaxSetting AjaxControlID="MenuSeaportList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuSeaportList" />
                        <telerik:AjaxUpdatedControl ControlID="rgvSeaport" UpdatePanelHeight="95%" />
                        <telerik:AjaxUpdatedControl ControlID="txtSeaportCodeSearch" />
                        <telerik:AjaxUpdatedControl ControlID="txtSeaportNameSearch" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvSeaport">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvSeaport" UpdatePanelHeight="95%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" RenderMode="Lightweight"></telerik:RadAjaxLoadingPanel>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
        <eluc:TabStrip ID="MenuSeaportList" runat="server" OnTabStripCommand="SeaportList_TabStripCommand">
        </eluc:TabStrip>
            </div>

            
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblSeaportCode" runat="server" Text="Seaport Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtSeaportCodeSearch" runat="server" Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblSeaportName" runat="server" Text="Seaport Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtSeaportNameSearch"  Text=""></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            
        <br clear="all" />
            
                <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="rgvSeaport" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvSeaport_NeedDataSource" OnPreRender="rgvSeaport_PreRender" OnItemCommand="rgvSeaport_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDSEAPORTID" >
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Code" UniqueName="CODE">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSeaportCode" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTCODE") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblID" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblSeaportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox"/>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                    </ClientSettings>
                </telerik:RadGrid>
    </form>
</body>
</html>
