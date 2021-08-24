<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsUserMenuAccess.aspx.cs" Inherits="OptionsUserMenuAccess" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Menu Permissions</title>
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <%-- <eluc:Title runat="server" ID="ucTitle" Text="User Menu Permissions" ShowMenu="false" />--%>
                 <eluc:TabStrip ID="MenuSecurityAccessRights" runat="server" OnTabStripCommand="SecurityAccessRights_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                
                <br clear="all" />
              
                 <telerik:RadGrid RenderMode="Lightweight" ID="gvMenu" Height="92%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvMenu_ItemCommand"
                ShowFooter="false" ShowHeader="true" EnableViewState="False" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
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
                        <telerik:GridTemplateColumn HeaderText="Menu Name">
                            <HeaderStyle Width="50%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblMenuCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUCODE") %>'></Telerik:RadLabel>
                                 <Telerik:RadLabel ID="lblMenuValue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUVALUE") %>'></Telerik:RadLabel>
                                <Telerik:RadLabel ID="lblMenuName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUNAME") %>'></Telerik:RadLabel>
                             </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30%"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50%"></ItemStyle>
                            <ItemTemplate>
                               
                                <asp:LinkButton runat="server" AlternateText="User Group" ToolTip="User Groups Mapped" Width="20PX" Height="20PX"
                                    CommandName="USERGROUPRIGHTS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdUserGroups">
                                <span class="icon"><i class="fa-usermap"></i></span>
                                </asp:LinkButton>
                                 <telerik:RadGrid ID="gvUserGroups" runat="server" ShowHeader="false" RowStyle-HorizontalAlign="Left" BorderColor="Red" RowStyle-BorderColor="Red"  AutoGenerateColumns="true">
                                                </telerik:RadGrid>      
                            </ItemTemplate>
                          
                        </telerik:GridTemplateColumn>
                    </Columns>
                  <%--  <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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


                                   