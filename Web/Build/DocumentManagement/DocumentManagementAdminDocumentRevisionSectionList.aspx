<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementAdminDocumentRevisionSectionList.aspx.cs" Inherits="DocumentManagementAdminDocumentRevisionSectionList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sections</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocumentRevision" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" Visible="false" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuDocument" runat="server" Title="Sections" OnTabStripCommand="MenuDocument_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <br />
            <table id="tblDocument" width="100%" align="center" runat="server">
                <tr>
                    <td>Document Revision
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDocumentRevision" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="120px">
                        </telerik:RadTextBox>
                    </td>
                    <td>Published Date
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPublishedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="120px">
                        </telerik:RadTextBox>
                    </td>
                    <td>Published By
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPublishedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="120px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuDocumentRevision" runat="server" OnTabStripCommand="MenuDocumentRevision_TabStripCommand"></eluc:TabStrip>


            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentRevision" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                Font-Size="11px" Width="100%" CellPadding="3" OnNeedDataSource="gvDocumentRevision_NeedDataSource" Height="80%" OnItemCommand="gvDocumentRevision_ItemCommand"
                OnItemDataBound="gvDocumentRevision_ItemDataBound" ShowFooter="true" ShowHeader="true"
                OnSelectedIndexChanging="gvDocumentRevision_SelectedIndexChanging" EnableViewState="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" EnableHeaderContextMenu="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Auto">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Section">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONREVISIONDETAILS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONID") %>'
                                    Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprovedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved By">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprovedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDBYNAME") %>'></telerik:RadLabel>
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
