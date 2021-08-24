<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterFMSDrawingList.aspx.cs"
    Inherits="RegisterFMSDrawingList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Drawing Category</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
        <style type="text/css">
            .hidden {
                display: none;
            }

            .center {
                background: fixed !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
            </telerik:RadWindowManager>
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divnote"></telerik:RadFormDecorator>
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Code">
                        </telerik:RadLabel>
                    </td>
                    <td style="padding-right: 40px">
                        <telerik:RadTextBox ID="txtShortCode" MaxLength="20" runat="server" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Name">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCategory" runat="server" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuMOCCategory" runat="server" OnTabStripCommand="MenuMOCCategory_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvFMSCategory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvFMSCategory_ItemCommand" OnItemDataBound="gvFMSCategory_ItemDataBound"
                ShowHeader="true" ShowFooter="false" EnableViewState="false" AllowSorting="true" OnSorting="gvFMSCategory_Sorting"
                Height="88%" AllowPaging="true" AllowCustomPaging="true" GridLines="None" OnNeedDataSource="gvFMSCategory_NeedDataSource"
                OnUpdateCommand="gvFMSCategory_UpdateCommand"
                RenderMode="Lightweight" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Sort Order">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsortcode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDORDER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                               <telerik:RadLabel ID="lbldrawingcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAWINGCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Drawing Name" FooterText="Drawing Category">
                            <HeaderStyle Width="14%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDrawingsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMSDRAWINGCATEGORY") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkDrawingsName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAWINGCATEGORY") %>'
                                    ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDDRAWINGCATEGORY") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="5" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
    </form>
</body>
</html>
