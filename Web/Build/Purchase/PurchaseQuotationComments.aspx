<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationComments.aspx.cs" Inherits="PurchaseQuotationComments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Discussion forum</title>
    
    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseComments" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuDiscussion">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuDiscussion" />
                        <telerik:AjaxUpdatedControl ControlID="rgvComment" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="txtNotesDescription" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvComment">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvComment" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
    

    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">--%>
            
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuDiscussion" runat="server" OnTabStripCommand="MenuDiscussion_TabStripCommand">
            </eluc:TabStrip>
                    </div>

                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <br />
                    <table cellpadding="1" cellspacing="1" width="100%"> 
                        <tr>
                            <td align="center">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtNotesDescription" runat="server" Rows="3" CssClass="gridinput_mandatory" TextMode="MultiLine" Resize="Both"
                                    EmptyMessage="Type your Comment here" Width="99%">

                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                <br />
                <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="rgvComment" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" 
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvComment_NeedDataSource" OnPreRender="rgvComment_PreRender">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Posted BY">
                                <ItemStyle Width="180px" />
                                <HeaderStyle Width="180px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPostedBY" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.NAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Comment">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblComment" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.DESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOldValue" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.POSTEDDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>

                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
     <%--</telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
