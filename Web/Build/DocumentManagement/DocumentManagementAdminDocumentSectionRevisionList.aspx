<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementAdminDocumentSectionRevisionList.aspx.cs" Inherits="DocumentManagementAdminDocumentSectionRevisionList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript">
            function fnConfirmTelerik(sender, msg) {
                var callBackFn = function (shouldSubmit) {
                    if (shouldSubmit) {
                        //sender.click();
                        //if (Telerik.Web.Browser.ff) {
                        //    sender.get_element().click();
                        //}
                        eval(sender.target.parentElement.parentElement.href);
                    }
                    else {
                        if (e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                var confirm;

                if (msg == null)
                    confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
                else
                    confirm = radconfirm(msg, callBackFn);

                return false;
            }
        </script>

    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentSectionRevison" runat="server" GridLines="None"
                Font-Size="11px" Width="100%" Height="88%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellPadding="3"
                OnNeedDataSource="gvDocumentSectionRevison_NeedDataSource" OnItemCommand="gvDocumentSectionRevison_ItemCommand"
                OnItemDataBound="gvDocumentSectionRevison_ItemDataBound" ShowFooter="true" ShowHeader="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDREVISONID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Added">
                            <HeaderTemplate>
                                Added
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkAddedDate" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblSectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisonId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONSTATUS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added By">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Added By
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revison">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Approved
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprovedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved By">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Approved By
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprovedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Draft">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Draft
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDraft" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAFTREVISION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit Content" CommandName="EDITCONTENT" ID="cmgEditContent"
                                    ToolTip="Edit Content"><span class="icon"><i class="fas fa-receipt"></i></span></asp:LinkButton>
                                <%-- <img id="Img4" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />--%>
                                <asp:LinkButton runat="server" AlternateText="View Content" CommandName="VIEWCONTENT" ID="cmgViewContent"
                                    ToolTip="View Content" Visible="false"><span class="icon"><i class="fas fa-receipt"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="APPROVE" CommandName="APPROVE" ID="cmdApprove"
                                    ToolTip="Approve"><span class="icon"><i class="fas fa-file"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="395px" SaveScrollPosition="true" FrozenColumnsCount="7" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
