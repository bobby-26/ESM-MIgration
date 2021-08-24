<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListDeliveryAddress.aspx.cs" Inherits="CommonPickListDeliveryAddress" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delivery Address</title>

    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDeliveryAddress" runat="server">
    
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ddlPurpose">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlPurpose" />
                        <telerik:AjaxUpdatedControl ControlID="rgvDeliveryAddress" UpdatePanelHeight="95%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvDeliveryAddress">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvDeliveryAddress" UpdatePanelHeight="95%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" RenderMode="Lightweight"></telerik:RadAjaxLoadingPanel>
    
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table width="100%">
                <tr>
                <td>
                    <telerik:RadLabel ID="lblPurpose" runat="server" RenderMode="Lightweight" Text="Purpose"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList RenderMode="Lightweight" ID="ddlPurpose" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlPurpose_SelectedIndexChanged">
                        </telerik:RadDropDownList>
                </td>
            </tr>
            </table>
                
            <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="rgvDeliveryAddress" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" Height="100%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvDeliveryAddress_NeedDataSource" OnItemCommand="rgvDeliveryAddress_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDADDRESSCONTACTID,FLDADDRESSCODE,FLDNAME" >
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkUserName" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItem %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblAddressContactID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCONTACTID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Department" UniqueName="DEPARTMENT">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Phone1" UniqueName="PHONE">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="EMAIL1" HeaderText="Email">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblEmail1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL1") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                           
                        </Columns>

<%--                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" />--%>
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
