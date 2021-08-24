<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseComments.aspx.cs" Inherits="PurchaseComments" %>

<!DOCTYPE html>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Purchase Broadcast</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseBroadcast" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuPhoenixBroadcast" runat="server" OnTabStripCommand="PhoenixBroadcast_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSearch" runat="server" Text="Search"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearch" runat="server" CssClass="input"></telerik:RadTextBox>
                        <asp:LinkButton ID="cmdAccountSearch" runat="server" OnClick="cmdSearchAccount_Click">
                            <span class="icon"><i class="fas fa-search"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
            <table width="100%" style="height: 100%; margin-top: 0px; vertical-align: top;">
                <tr>
                    <td width="20%" valign="top">
                        <eluc:TabStrip ID="MenuUser" runat="server" OnTabStripCommand="MenuUser_TabStripCommand"></eluc:TabStrip>
                        <div style="border-width: 1px; border-style: groove; border-color: Navy; height: 100%; width: 100%; overflow-x: hidden; overflow-y: auto;">
                            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvUsers" DecoratedControls="All" EnableRoundedCorners="true" />
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvUsers" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Width="100%"
                                CellSpacing="0" GridLines="None" OnNeedDataSource="gvUsers_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                OnItemDataBound="gvUsers_ItemDataBound"
                                OnItemCommand="gvUsers_ItemCommand"
                                OnDeleteCommand="gvUsers_DeleteCommand">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDID" ShowFooter="true">
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="User" HeaderStyle-Width="115px" AllowSorting="true" SortExpression="FLDTOUSER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToUserId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOUSER") %>'></asp:Label>
                                                <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                                <asp:LinkButton ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOUSERNAME") + " (" + DataBinder.Eval(Container,"DataItem.FLDMYMSGCOUNT") + ") (" + DataBinder.Eval(Container,"DataItem.FLDHISMSGCOUNT") + ") " %>'
                                                    CommandName="SELECTUSER"></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                            <HeaderStyle Width="40px" />
                                            <ItemStyle Width="40px" Wrap="false" />
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found" PageSizeLabelText="Records per page:" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>
                    </td>
                    <td rowspan="2" valign="top">
                        <div style="border-width: 1px; border-style: groove; border-color: Navy; height: 100%; width: 100%; overflow: hidden;">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox CssClass="input" runat="server" ID="txtSubject" MaxLength="200" Width="80%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblMessage" runat="server" Text="Message"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox CssClass="input" runat="server" ID="txtMessage" TextMode="MultiLine" Rows="5" Columns="80" MaxLength="500" Width="80%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvPhoenixNews" DecoratedControls="All" EnableRoundedCorners="true" />
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvPhoenixNews" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                CellSpacing="0" GridLines="None" OnNeedDataSource="gvPhoenixNews_NeedDataSource" BorderColor="Transparent" CellPadding="3" Width="100%"
                                OnItemDataBound="gvPhoenixNews_ItemDataBound" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                OnItemCommand="gvPhoenixNews_ItemCommand">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDID" ShowFooter="true">
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Message" HeaderStyle-Width="80%" ItemStyle-Width="80%">
                                            <ItemTemplate>
                                                <table width="100%" cellpadding="2" cellspacing="2">
                                                    <tr>
                                                        <td width="10%">
                                                            <asp:LinkButton runat="server" ID="lblSubject" Font-Size="Larger" Font-Bold="true" CommandName="EDIT"
                                                                ForeColor="Navy" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>'></asp:LinkButton>
                                                            <asp:Label runat="server" ID="lblId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                                            <asp:Label runat="server" ID="lblRead" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREADYN") %>'></asp:Label>
                                                            <asp:Label runat="server" ID="lblToUser" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOUSER") %>'></asp:Label>
                                                        </td>
                                                        <td>[
                                                            <asp:Label runat="server" ID="lblUser" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %>'></asp:Label>
                                                            ]
                                                            <asp:Label runat="server" ID="lblMessage" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMESSAGE") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <table width="100%" cellpadding="2" cellspacing="2">
                                                    <tr>
                                                        <td width="10%">
                                                            <asp:LinkButton runat="server" ID="lblSubjectEdit" Font-Size="Larger" Font-Bold="true" CommandName="CANCEL"
                                                                ForeColor="Navy" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>'></asp:LinkButton>
                                                        </td>
                                                        <td>[
                                                            <asp:Label runat="server" ID="lblUserEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %>'></asp:Label>
                                                            ]
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDateEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") + " (GMT)" %>'></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="lblMessageEdit" Width="100%" CssClass="input" Height="60px" ReadOnly="true" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMESSAGE") %>'></asp:TextBox></td>
                                                        <%--<asp:Label runat="server" ID="lblMessageEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMESSAGE") %>'></asp:Label>--%>
                                                    </tr>
                                                </table>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Reply" ImageUrl="<%$ PhoenixTheme:images/communication.png %>"
                                                    CommandName="REPLY" ID="cmdReply" ToolTip="Reply"></asp:ImageButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Reply" ImageUrl="<%$ PhoenixTheme:images/communication.png %>"
                                                    CommandName="REPLYEDIT" ID="cmdReplyEdit" ToolTip="Reply"></asp:ImageButton>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found" PageSizeLabelText="Records per page:"
                                        CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
        <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
    </form>
</body>
</html>
