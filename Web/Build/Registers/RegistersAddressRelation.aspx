<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddressRelation.aspx.cs" Inherits="RegistersAddressRelation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%--<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuAddressRelationMapping" runat="server" OnTabStripCommand="MenuAddressRelationMapping_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            </div>
            <div id="divFind" style="position: relative; z-index: 2">
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCode" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAddressName" runat="server" MaxLength="200" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Country ID="ucCountry" runat="server" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:TabStrip ID="MenuAddressRelationSearch" runat="server" OnTabStripCommand="MenuAddressRelationSearch_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" Height="92.5%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"  OnNeedDataSource="RadGrid1_NeedDataSource"
                OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCODE" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code" HeaderStyle-Width="100px" AllowSorting="true" SortExpression="FLDCODE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="100px" AllowSorting="true" SortExpression="FLDNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddressCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAddressName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Phone 1" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="City" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITY") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Country" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCountryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblstatusId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="ADDADDRESS" ID="cmdAdd" ToolTip="Relate Address">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span> 
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns> 
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <%--            <div id="divGrid" style="position: relative;">
                <asp:GridView ID="gvAddressMapping" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Font-Size="11px" OnRowCommand="gvAddressMapping_RowCommand" OnRowDataBound="gvAddressMapping_RowDataBound"
                    OnRowDeleting="gvAddressMapping_RowDeleting" AllowSorting="false"
                    ShowFooter="false" ShowHeader="true" Width="100%"
                    OnRowEditing="gvAddressMapping_RowEditing">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblCodeHeader" runat="server">Code </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblNameHeader" runat="server">Name </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkAddressName" runat="server" CommandArgument='<%# Bind("FLDADDRESSCODE") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' Visible="false"></asp:LinkButton>
                                <asp:Label ID="lblAddressCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblAddressName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblPhone1Header" runat="server">Phone 1  </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblCityHeader" runat="server"> City </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITY") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblCountryHeader" runat="server"> Country </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCountryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblSatusHeader" runat="server"> Status </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblstatusId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Add Address" ImageUrl="<%$ PhoenixTheme:images/add.png %>"
                                    CommandName="ADDADDRESS" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                    ToolTip="Relate Address"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>--%>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
