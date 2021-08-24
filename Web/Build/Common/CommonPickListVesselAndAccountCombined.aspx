<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListVesselAndAccountCombined.aspx.cs" Inherits="CommonPickListVesselAndAccountCombined" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
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
      <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
  <%--  <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">--%>
<%--            <telerik:RadLabel ID="lblAccount" runat="server" Text="Account"></telerik:RadLabel>--%>
<%--        </div>--%>
<%--    </div>--%>
<%--    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">--%>
        <eluc:TabStrip ID="MenuAccount" runat="server" OnTabStripCommand="MenuAccount_TabStripCommand">
        </eluc:TabStrip>
<%--    </div>--%>
    <br clear="all" />
    <telerik:RadAjaxPanel runat="server" ID="pnlAccount">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--            <div id="search">--%>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtVesselName" CssClass="input" MaxLength="100"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAccountCodeSearch" CssClass="input" runat="server" MaxLength="100" ></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAccountDescription" runat="server" Text="Account Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAccountDescSearch" CssClass="input" runat="server" MaxLength="100" ></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
<%--            </div>--%>
<%--            <div id="divGrid" style="position: relative;">--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAccount" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Font-Size="11px" OnItemCommand="gvAccount_ItemCommand" OnItemDataBound="gvAccount_ItemDataBound" OnNeedDataSource="gvAccount_NeedDataSource"
                     ShowFooter="false" ShowHeader="true" Width="100%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" 
                    OnSortCommand="gvAccount_SortCommand"  EnableViewState="false">
                     <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="353px">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblVesselName" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="353px">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAccountId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblVesselId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkAccountCode" runat="server" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="353px">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAccountDescriptionHeader" runat="server">Account Description</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAccountDescription" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                          <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                         </MasterTableView>
                     <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                </telerik:RadGrid>
         
    <%--    <Triggers>
            <asp:PostBackTrigger ControlID="gvAccount" />
        </Triggers>--%>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
