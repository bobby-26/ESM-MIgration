<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListCity.aspx.cs"
    Inherits="CommonPickListCity" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>City</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuCity" runat="server" OnTabStripCommand="MenuCity_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>

        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div id="search">
            <table id="tblConfigureCity" width="100%">
                <tr>
                    <td width="33.33">
                        <eluc:Country ID="ddlcountrylist" runat="server" AppendDataBoundItems="true" 
                            AutoPostBack="true" OnTextChangedEvent="RadDropDownCountry_TextChangedEvent" Width="45%" />
                    </td>
                    <td width="33.33">
                        <eluc:State runat="server" ID="ucState" AppendDataBoundItems="true" 
                            AutoPostBack="true" OnTextChangedEvent="RadDropDownState_TextChangedEvent" Width="45%" />
                    </td>
                    <td width="33.33">
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input" EmptyMessage="Type the city" Width="45%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <table id="tblfrequwent" width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>&nbsp;<b><telerik:RadLabel ID="lblMostfrequentlyusedcitiesasOrigin" runat="server" Text="Most frequently used cities as Origin"></telerik:RadLabel>
                </b>
                </td>
                <td>&nbsp;
                </td>
                <td>&nbsp;<b><telerik:RadLabel ID="lblMostfrequentlyusedcitiesasDestination" runat="server" Text="Most frequently used cities as Destination"></telerik:RadLabel>
                </b>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOriginCity" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="true" OnItemCommand="gvOriginCity_ItemCommand">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDCITYID" TableLayout="Fixed">
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="City" AllowSorting="true" SortExpression="FLDCITYNAME">
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblorgCityid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkorgCityName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="State" ColumnGroupName="State" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSTATENAME">
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblorgstateName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Country" ColumnGroupName="Country" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOUNTRYNAME">
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblorgcountryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
                <td>&nbsp;
                </td>
                <td>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvDestinationCity" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="true" OnItemCommand="gvDestinationCity_ItemCommand">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDCITYID" TableLayout="Fixed">
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="City" AllowSorting="true" SortExpression="FLDCITYNAME">
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbldestCityid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkdestCityName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="State" ColumnGroupName="State" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSTATENAME">
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbldeststateName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Country" ColumnGroupName="Country" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOUNTRYNAME">
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbldestcountryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>

        &nbsp;<b><telerik:RadLabel ID="lblCityList" runat="server" Text="City List"></telerik:RadLabel>
        </b>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCity" runat="server" AutoGenerateColumns="false" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="true" OnItemCommand="gvCity_ItemCommand" AllowSorting="true"
            OnRowDataBound="gvCity_ItemDataBound" OnNeedDataSource="gvCity_NeedDataSource" OnSortCommand="gvCity_SortCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDCITYID" TableLayout="Fixed">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="City" SortExpression="FLDCITYNAME">
                        <HeaderStyle Width="10%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCityid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkCityName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>' CommandName="PICKCITY"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="State" AllowSorting="true" SortExpression="">
                        <HeaderStyle Width="10%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStateId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCountryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Country" AllowSorting="true" SortExpression="">
                        <HeaderStyle Width="10%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCountryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lnkairportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
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

    </form>
</body>
</html>
