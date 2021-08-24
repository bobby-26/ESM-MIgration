<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsUserGroupList.aspx.cs"
    Inherits="Options_OptionsUserGroupList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Group</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
   
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="98%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <%-- <eluc:Title runat="server" ID="Title1" Text="User - Group" ShowMenu="false" />--%>
            <eluc:TabStrip ID="MenuUserAdmin" runat="server" OnTabStripCommand="UserAdmin_TabStripCommand" >
                        </eluc:TabStrip>           
                  
            <telerik:RadGrid RenderMode="Lightweight" ID="dgUserGroups" Height="92%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnSorting="dgUserGroups_Sorting"
                CellSpacing="0" GridLines="None" OnItemCommand="dgUserGroups_ItemCommand"  OnNeedDataSource="dgUserGroups_NeedDataSource" OnItemDataBound="dgUserGroups_ItemDataBound"
                ShowFooter="False" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn >
                            <HeaderStyle Width="30%" />
                              <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%-- <telerik:RadCheckBox runat="server" ID="chkGroupRights" OnCheckedChanged="CheckBoxClicked" AutoPostBack="true"  BackColor="Transparent" ForeColor="Transparent" />--%>
                                 <%-- <asp:CheckBox runat="server" ID="chkGroupRights" OnCheckedChanged="CheckBoxClicked" AutoPostBack="true"  EnableViewState="true"/>--%>
                                 <Telerik:RadcheckBox runat="server" ID="chkGroupRights" CommandName="UPDATE"
                                        AutoPostBack="true" EnableViewState="true"/>
                                 <%-- <asp:CheckBox runat="server" ID="chkGroupRights" AutoPostBack="true"  BackColor="Transparent" ForeColor="Transparent" ></asp:CheckBox>--%>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group Name" AllowSorting="true" SortExpression="FLDGROUPNAME">
                            <HeaderStyle Width="70%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblGroupcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPCODE") %>'></Telerik:RadLabel>
                                    <asp:LinkButton ID="lnkGroupName" runat="server" CommandName="Select"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                    <Telerik:RadLabel ID="lblGroupcodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPCODE") %>'></Telerik:RadLabel>
                                    <Telerik:RadTextBox ID="txtGroupNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></Telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDocumentFieldsAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" ToolTip="Enter Document Field ">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
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
