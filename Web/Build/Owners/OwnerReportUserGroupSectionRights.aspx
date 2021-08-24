<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerReportUserGroupSectionRights.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="OwnerReportUserGroupSectionRights" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Module Configuration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvModule.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:TabStrip ID="MenuOptionmodule" runat="server" OnTabStripCommand="MenuOptionmodule_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUserGroup" runat="server" Text="User Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserGroup runat="server" ID="ucUserGroup" AutoPostBack="true" Width="420px" OnTextChangedEvent="ucUserGroup_TextChangedEvent" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>

            <telerik:RadGrid ID="gvModule" runat="server" AutoGenerateColumns="False" Width="100%"
                AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="false" OnItemCommand="gvModule_ItemCommand"
                OnNeedDataSource="gvModule_NeedDataSource" OnItemDataBound="gvModule_ItemDataBound">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSECTIONID">
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDORCATEGORYNAME" SortOrder="Ascending" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDORCATORDER" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Section">
                            <HeaderStyle Width="70%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblModuleName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblModuleId" ToolTip="measurecode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Include YN">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>

                                <telerik:RadCheckBox runat="server" AutoPostBack="true" EnableViewState="true" ID="chkModuleRights" CommandName="UPDATE" />
                                <telerik:RadLabel runat="server" ID="lblRights" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRIGHTS") %>'></telerik:RadLabel>

                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
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
