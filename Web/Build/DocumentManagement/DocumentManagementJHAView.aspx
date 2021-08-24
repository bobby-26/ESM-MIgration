<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementJHAView.aspx.cs" Inherits="DocumentManagementJHAView" %>

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
                    TelerikGridResize($find("<%= gvJobHazard.ClientID %>"));
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
        <telerik:RadGrid RenderMode="Lightweight" ID="gvJobHazard" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" OnNeedDataSource="gvJobHazard_NeedDataSource"
                OnItemDataBound="gvJobHazard_RowDataBound"  ShowFooter="false" EnableViewState="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDJOBHAZARDID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="35%"></ItemStyle>
                            <HeaderTemplate>
                                Process
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50%"></ItemStyle>
                            <HeaderTemplate>
                                Job
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB").ToString().Length > 42 ? (DataBinder.Eval(Container, "DataItem.FLDJOB").ToString().Substring(0, 42) + "...") : DataBinder.Eval(Container, "DataItem.FLDJOB").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucJob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Ref Number
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="JHASELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblJobHazardId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
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