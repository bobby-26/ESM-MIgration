<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListMultipleHard.aspx.cs" Inherits="CommonPickListMultipleHard" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hard</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersUser" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadTextBox runat="server" ID="txtTitle" CssClass="readonlltextbox" Enabled="false" Width="100%"></telerik:RadTextBox>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuHard" runat="server" OnTabStripCommand="Hard_TabStripCommand"></eluc:TabStrip>
            <%--<eluc:TabStrip ID="MenuHardGrid" runat="server" OnTabStripCommand="HardGrid_TabStripCommand"></eluc:TabStrip>--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvHard" Height="86%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvHard_ItemCommand" OnItemDataBound="gvHard_ItemDataBound"
                ShowHeader="true" OnSortCommand="gvHard_SortCommand" OnNeedDataSource="gvHard_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDHARDTYPECODE">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Select">
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkSelect" />
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Code">
                                <HeaderStyle Width="45%" />
                                <Itemstyle wrap="False" horizontalalign="Left"></Itemstyle>
                                <Headertemplate>
                                    <telerik:RadLabel ID="lblUserHeader" runat="server">Code&nbsp;</telerik:RadLabel>
                                </Headertemplate>
                                <Itemtemplate>
                                    <telerik:RadLabel ID="lblHardTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDTYPECODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblHardCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblShortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                                </Itemtemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Name">
                                <HeaderStyle Width="45%" />
                                <Itemstyle wrap="False" horizontalalign="Left"></Itemstyle>
                                <Itemtemplate>
                                    <telerik:RadLabel ID="lblHardName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                </Itemtemplate>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
