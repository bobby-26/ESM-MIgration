<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListCompanyAccount.aspx.cs" Inherits="CommonPickListCompanyAccount" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Account List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
    

<%--            <asp:Literal ID="lblAccount" runat="server" Text="Account"></asp:Literal>--%>

        <eluc:TabStrip ID="MenuAccount" runat="server" OnTabStripCommand="MenuAccount_TabStripCommand">
        </eluc:TabStrip>
    
    <table>
    <tr>
    
    <td><asp:Literal ID="lblAccountCodeDescription" runat="server" Text="Account Code/ Description"></asp:Literal></td>
    <td> <asp:TextBox ID="txtAccountcode" MaxLength="50" Width="200px" runat="server" CssClass="input">
                            </asp:TextBox></td>
    </tr></table>
    <telerik:RadAjaxPanel runat="server" ID="pnlAccount" Height="87%"  >
         <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAccount" runat="server" AutoGenerateColumns="False" CellPadding="3" Height="95%"
                    Font-Size="11px" OnItemCommand="gvAccount_ItemCommand" OnItemDataBound="gvAccount_ItemDataBound" OnNeedDataSource="gvAccount_NeedDataSource"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    ShowFooter="false" ShowHeader="true" Width="100%"
                    EnableViewState="false"  OnSortCommand="gvAccount_SortCommand">
                     <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />

                    <Columns>
                        <telerik:GridTemplateColumn>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                               <telerik:RadLabel ID="lblAccountcodeHeader" runat="server">Account code</telerik:RadLabel>

                                <img id="FLDACCOUNTCODE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAccountId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <%--<asp:LinkButton ID="lnkAccountCode" runat="server" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></asp:LinkButton>--%>
                                 <asp:LinkButton ID="lnkAccountCode" runat="server" 
                                      Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>' CommandName="ACCOUNT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAccountDescriptionHeader" runat="server">Account Description</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAccountDescription" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                       <%-- <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblAccountTypeHeader" runat="server">Account Type</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAccountType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTTYPE") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                     <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                      <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true"  />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"   />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                </telerik:RadGrid>
      
<%--        <Triggers>
            <asp:PostBackTrigger ControlID="gvAccount" />
        </Triggers>--%>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>

