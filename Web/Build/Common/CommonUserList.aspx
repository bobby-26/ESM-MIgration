<%@ Page Language="C#" AutoEventWireup="True" CodeFile="CommonUserList.aspx.cs"
    Inherits="CommonUserList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    </head>
<body>
  
     <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
      <form id="frmRegistersUser" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>          
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                 
                    <table id="tblConfigureUser" width="100%">
                        <tr>
                            <td>
                                <Telerik:RadLabel ID="lblUserName" runat="server" Text="User Name"></Telerik:RadLabel>
                            </td>
                            <td>
                                <Telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input" Width="240px"></Telerik:RadTextBox>
                            </td>
                            <td>
                                <Telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></Telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Department runat="server" ID="ucDepartment" CssClass="input" AppendDataBoundItems="true" Width="240px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <Telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></Telerik:RadLabel>
                            </td>
                            <td>
                                <Telerik:RadTextBox runat="server" ID="txtFirstName" MaxLength="100" CssClass="input" Width="240px"></Telerik:RadTextBox>
                            </td>
                            <td>
                                <Telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></Telerik:RadLabel>
                            </td>
                            <td>
                                <Telerik:RadTextBox runat="server" ID="txtMiddleName" MaxLength="100" CssClass="input" Width="240px"></Telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <Telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></Telerik:RadLabel>
                            </td>
                            <td>
                                <Telerik:RadTextBox runat="server" ID="txtLastName" MaxLength="100" CssClass="input" Width="240px"></Telerik:RadTextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>                            
                        </tr>
                    </table>
              <eluc:TabStrip ID="MenuSecurityUsers" runat="server" OnTabStripCommand="MenuSecurityUsers_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="dgUser" Height="81%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" 
                CellSpacing="0" GridLines="None" OnItemCommand="dgUser_ItemCommand" OnNeedDataSource="dgUser_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="Department" AllowSorting="true" >
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <Telerik:RadLabel ID="lblUser" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></Telerik:RadLabel>
                                  <Telerik:RadLabel ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Username" AllowSorting="true" SortExpression="FLDUSERNAME">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                               <Telerik:RadLabel ID="lblUserCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></Telerik:RadLabel>
                                    <asp:LinkButton ID="lnkUserName" runat="server" CommandName="CHOOSE" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                           </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Short Code">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblShortCode" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                           
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Group List" >
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblGroupList" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPLIST") %>' Visible="false"></Telerik:RadLabel>
                                    <img ID="imgGroupList" runat="server" src="<%$ PhoenixTheme:images/te_view.png %>"  onmousedown="javascript:closeMoreInformation()" />
                            </ItemTemplate>
                           </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Active YN" >
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                               <Telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYNTEXT").ToString() %>'></Telerik:RadLabel>
                            </ItemTemplate>
                           </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="First Name" >
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                              <Telerik:RadLabel ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                           </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Last Name"  >
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                               <Telerik:RadLabel ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                           </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Middle Name"  >
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblMiddleName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME") %>'></Telerik:RadLabel>                                    
                            </ItemTemplate>
                           </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText=" Effective From"  >
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblEffectiveFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEFROM", "{0:dd/MMM/yyyy}") %>'></Telerik:RadLabel>
                            </ItemTemplate>
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
           
               
               
                   
                
<%--                    <asp:GridView ID="dgUser" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="dgUser_RowCommand" OnRowDataBound="dgUser_RowDataBound"
                        ShowHeader="true" AllowSorting="true" OnSorting = "dgUser_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />--%>
                       
                           
                              
                            
                         
                           
                   