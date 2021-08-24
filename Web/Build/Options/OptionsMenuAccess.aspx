<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsMenuAccess.aspx.cs"
    Inherits="OptionsMenuAccess" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Menu Access</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuSecurityAccessRights" runat="server" OnTabStripCommand="SecurityAccessRights_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUserGroup" runat="server" Text="User Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserGroup runat="server" ID="ucUserGroup" AutoPostBack="true" OnTextChangedEvent="ucUserGroup_TextChanged" Width="320px" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblApplication" runat="server" Text="Application"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox runat="server" ID="ddlAdministratorMenuList" AutoPostBack="true" Width="180px" OnTextChanged="ddlAdministratorMenuList_TextChanged" AppendDataBoundItems="true" DataTextField="FLDMENUNAME" DataValueField="FLDMENUCODE">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>

                </tr>
            </table>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvMenu" Height="91%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvMenu_ItemCommand" OnItemDataBound="gvMenu_ItemDataBound" OnNeedDataSource="gvMenu_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%--  <Telerik:RadCheckBox runat="server" ID="chkMenuRights" OnCheckedChanged="CheckBoxClicked" AutoPostBack="true"  BackColor="Transparent" ForeColor="Transparent" />--%>
                                <telerik:RadCheckBox runat="server" ID="chkMenuRights" CommandName="UPDATE"
                                    AutoPostBack="true" EnableViewState="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Menu Name">
                            <HeaderStyle Width="70%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMenuCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMenuName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Page Rights" ToolTip="Page Rights" Width="20PX" Height="20PX"
                                    CommandName="PAGERIGHTS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPageRights">
                                <span class="icon"><i class="fa-28"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="User Group" ToolTip="User Groups Mapped" Width="20PX" Height="20PX"
                                    CommandName="USERGROUPRIGHTS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdUserGroups">
                                <span class="icon"><i class="fa-usermap"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <%-- <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />--%>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
