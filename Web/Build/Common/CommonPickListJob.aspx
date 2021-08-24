<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListJob.aspx.cs"
    Inherits="CommonPickListJob" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuJob" runat="server" OnTabStripCommand="MenuJob_TabStripCommand"></eluc:TabStrip>
            <br clear="all" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Literal ID="lblJobCode" runat="server" Text="Job Code"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtJobCode" CssClass="input" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblJobTitle" runat="server" Text="Job Title"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtJobTitle" CssClass="input" MaxLength="200" Width="240px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblJobClass" runat="server" Text="Job Class"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick ID="ucJobClass" runat="server" CssClass="input" QuickTypeCode="34" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvJob" runat="server" Height="80%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" EnableViewState="false" GridLines="None" Font-Size="11px" OnItemCommand="gvJob_ItemCommand" OnSortCommand="gvJob_SortCommand"
                OnNeedDataSource="gvJob_NeedDataSource" ShowFooter="False" ShowHeader="true" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDJOBID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDJOBCODE">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbljobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Job Title" AllowSorting="true" SortExpression="FLDJOBTITLE">
                            <HeaderStyle Width="22%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>' CommandName="Select"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Job Class" AllowSorting="true" SortExpression="FLDCLASS">
                            <HeaderStyle Width="22%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
