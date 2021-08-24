<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementSectionView.aspx.cs" Inherits="DocumentManagementSectionView" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSectionMatches.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSectionMatches" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" OnNeedDataSource="gvSectionMatches_NeedDataSource"
                OnItemDataBound="gvSectionMatches_RowDataBound" EnableViewState="false" ShowFooter="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDSECTIONID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width="24%" />
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="29%" />
                            <HeaderTemplate>
                                Subcategory
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME").ToString().Length > 39 ? (DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString().Substring(0, 39) + "...") : DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucDocumentName" TargetControlId="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="44%" />
                            <HeaderTemplate>
                                Section
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSectionName" runat="server" CommandName="SECTIONSELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION").ToString().Length > 65 ? (DataBinder.Eval(Container, "DataItem.FLDSECTION").ToString().Substring(0, 65) + "...") : DataBinder.Eval(Container, "DataItem.FLDSECTION").ToString() %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblSectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucSectionName" TargetControlId="lnkSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION") %>' Width="400px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                            <HeaderTemplate>
                                Action                               
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="ReadUnread" CommandName="READUNREAD" ID="lnkReadUnreadSec"
                                    ToolTip="Read/Unread List"><span class="icon"><i class="fa-coins-ei"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Forms and Checklist" CommandName="FORMS" ID="cmdForms"
                                    ToolTip="Linked Forms and Checklist"><span class="icon"><i class="fa-file-contract-af"></i></span></asp:LinkButton>
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
                </MasterTableView>
                <ClientSettings>
                    <Resizing AllowColumnResize="true" />
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
