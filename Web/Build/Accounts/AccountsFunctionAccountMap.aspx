<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsFunctionAccountMap.aspx.cs"
    Inherits="AccountsFunctionAccountMap" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PageNumber" Src="~/UserControls/UserControlPageNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCompanyAccountMap" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text=""></eluc:Status>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand"
                    TabStrip="True"></eluc:TabStrip>
            </div>
            <eluc:TabStrip ID="MenuCompanyAccountMap" runat="server" OnTabStripCommand="MenuCompanyAccountMap_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCompanyAccountMap" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvCompanyAccountMap_ItemDataBound"
                OnRowUpdating="gvCompanyAccountMap_RowUpdating" OnNeedDataSource="gvCompanyAccountMap_NeedDataSource"
                OnUpdateCommand="gvCompanyAccountMap_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDACCOUNTMAPID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Module" AllowSorting="true" SortExpression="FLDMODULE">
                            <ItemStyle HorizontalAlign="left" Wrap="False" Width="200px" />
                            <FooterStyle HorizontalAlign="left" Wrap="False" Width="200px" />
                            <HeaderStyle HorizontalAlign="left" Wrap="False" Width="200px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblModuleItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODULE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Function" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                            <ItemStyle HorizontalAlign="left" Wrap="False" Width="200px" />
                            <FooterStyle HorizontalAlign="left" Wrap="False" Width="200px" />
                            <HeaderStyle HorizontalAlign="left" Wrap="False" Width="200px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFunctinItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountMapId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTMAPID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                    Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account" AllowSorting="true" SortExpression="FLDACCOUNTCODE">
                            <ItemStyle HorizontalAlign="left" Wrap="False" Width="300px" />
                            <FooterStyle HorizontalAlign="left" Wrap="False" Width="300px" />
                            <HeaderStyle HorizontalAlign="left" Wrap="False" Width="300px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListExpenseAccount">
                                    <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input" ReadOnly="false"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>' MaxLength="10"
                                        Width="20%"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTDESCRIPTION") %>'
                                        MaxLength="50"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowAccountPiclist" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="10" Width="0px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountSource" CssClass="readonlytextbox" runat="server" Width="0px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountUsage" CssClass="readonlytextbox" runat="server" Width="0px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" AllowSorting="true" SortExpression="FLDACCOUNTDESCRIPTION">
                            <ItemStyle HorizontalAlign="left" Wrap="False" Width="300px" />
                            <FooterStyle HorizontalAlign="left" Wrap="False" Width="300px" />
                            <HeaderStyle HorizontalAlign="left" Wrap="False" Width="300px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="true" SortExpression="FLDREMARKS">
                            <ItemStyle HorizontalAlign="left" Wrap="False" Width="400px" />
                            <FooterStyle HorizontalAlign="left" Wrap="False" Width="400px" />
                            <HeaderStyle HorizontalAlign="left" Wrap="False" Width="400px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    CssClass="input" Width="400px"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Wrap="False" Width="100px" />
                            <FooterStyle HorizontalAlign="Center" Wrap="False" Width="100px" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="100px" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
