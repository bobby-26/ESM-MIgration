<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentByCategoryList.aspx.cs" Inherits="DocumentManagementDocumentByCategoryList" %>

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
    <title>Document By Category</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="Div1" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

            <script type="text/javascript" language="javascript">
                function SetParentIframeURL(type, url, selectednode) {
                    if ((type == 5 || type ==6)) {
                        openNewWindow(type, '', url, selectednode);
                    }
                    else {
                        window.parent.SetSourceURL(type, url, selectednode);
                    }

                    
                }
            </script>
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocumentByCategory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <%--<eluc:Title runat="server" ID="ucTitle" Text="File List" ShowMenu="false"></eluc:Title>--%>
            <eluc:TabStrip ID="MenuDocument" Title="File List" runat="server"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDocumentByCategory" runat="server" OnTabStripCommand="MenuDocumentByCategory_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentByCategory" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                OnNeedDataSource="gvDocumentByCategory_NeedDataSource" OnItemCommand="gvDocumentByCategory_RowCommand"
                OnItemDataBound="gvDocumentByCategory_ItemDataBound" ShowFooter="false" ShowHeader="true" EnableViewState="true" 
                OnSelectedIndexChanging="gvDocumentByCategory_SelectedIndexChanging">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDFILEID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File Name">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                File Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkFileName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>' OnClientClick='<%# string.Format("javascript:SetParentIframeURL(\"{0}\",\"{2}\",\"{3}\")", Eval("FLDTYPE"), "",GetParentIframeURL(Eval("FLDFILEID").ToString()), Eval("FLDFILEID")) %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblFileId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision">
                            <HeaderStyle Width="125px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevisionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Remarks
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'></telerik:RadLabel>
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
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
