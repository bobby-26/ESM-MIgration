<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormView.aspx.cs" Inherits="DocumentManagementFormView" %>

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
                    TelerikGridResize($find("<%= gvFormMatch.ClientID %>"));
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
            <telerik:RadGrid ID="gvFormMatch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="false" AllowPaging="false"
                OnNeedDataSource="gvFormMatch_NeedDataSource" OnItemDataBound="gvFormMatch_RowDataBound" ShowFooter="false" EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDFORMID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Top Category">
                            <HeaderStyle Width="25%" />
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTopCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <HeaderStyle Width="25%" />
                            <HeaderTemplate>
                                Subcategory
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="45%" />
                            <HeaderTemplate>
                                Forms / Checklists / Circulars
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkFormName" runat="server" CommandName="FORMSELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME").ToString().Length > 65 ? (DataBinder.Eval(Container, "DataItem.FLDNAME").ToString().Substring(0, 65) + "...") : DataBinder.Eval(Container, "DataItem.FLDNAME").ToString() %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucFormName" TargetControlId="lnkFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' Width="400px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Documents" CommandName="DOCUMENTS" ID="cmdDocuments"
                                    ToolTip="Linked Documents"><span class="icon"><i class="fas fa-proposeST"></i></span></asp:LinkButton>
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
