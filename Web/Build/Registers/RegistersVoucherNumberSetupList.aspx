<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVoucherNumberSetupList.aspx.cs"
    Inherits="RegistersVoucherNumberSetupList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="VoucherNumberSetup">
    <title>Voucher Number Format</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVoucherNumberSetupList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuVoucherNumberSetup" runat="server" OnTabStripCommand="MenuVoucherNumberSetup_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" Height="93%" ID="gvVoucherNumberSetup" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" Font-Size="11px" OnRowDataBound="gvVoucherNumberSetup_ItemDataBound" OnItemCommand="gvVoucherNumberSetup_ItemCommand"
                OnNeedDataSource="gvVoucherNumberSetup_NeedDataSource" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Voucher" AllowSorting="true" SortExpression="FLDVOUCHERTYPECODE">
                            <HeaderStyle Width="25%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoucherType" runat="server" Width="200px" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDVOUCHERTYPE")%>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="hidtxtSubVoucherCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBVOUCHERTYPECODE")%>'
                                    Visible="false">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="hidtxtVoucherNumberFormatCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOUCHERTYPECODE")%>'
                                    Visible="false">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Sub Voucher" AllowSorting="true" SortExpression="FLDSUBVOUCHERTYPE">
                                    <HeaderStyle Width="25%" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSubVoucherType" runat="server" Width="200px"
                                            CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBVOUCHERTYPE") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDVOUCHERTYPEPREFIX">
                            <HeaderStyle Width="25%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoucherTypeCode" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERTYPEPREFIX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtVoucherTypeEdit" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERTYPEPREFIX") %>'
                                    CssClass="gridinput_mandatory" MaxLength="2">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                    <HeaderStyle Width="25%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                    <ItemTemplate>
                                        <telerik:RadImageButton ID="cmdEdit" runat="server" AlternateText="Edit"
                                            CommandName="EDIT" Image-Url="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" Width="20px" Height="20px" />
                                        <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <telerik:RadImageButton ID="cmdSave" runat="server" AlternateText="Save"
                                            CommandName="Save" Image-Url="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" Width="20px" Height="20px" />
                                        <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <telerik:RadImageButton runat="server" AlternateText="Cancel" Image-Url="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" ID="cmdCancel"
                                            ToolTip="Cancel" Width="20px" Height="20px"></telerik:RadImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>--%>
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
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
    </form>
</body>
</html>
