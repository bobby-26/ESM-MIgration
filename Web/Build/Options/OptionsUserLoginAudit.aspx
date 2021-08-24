<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsUserLoginAudit.aspx.cs" Inherits="OptionsUserLoginAudit" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date"  Src="~/UserControls/UserControlDate.ascx" %>


<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Login Audit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
  <form id="frmUserIdentity" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
         <%-- <eluc:Title runat="server" ID="ucTitle" Text="User Login Audit"  ShowMenu="false"/>--%>
                 <eluc:TabStrip ID="MenuRemoteUser" runat="server" OnTabStripCommand="MenuRemoteUser_TabStripCommand" TabStrip="true">
            </eluc:TabStrip>
     <eluc:TabStrip ID="MenuuserLoginAudit" runat="server" OnTabStripCommand="MenuuserLoginAudit_TabStripCommand">
                    </eluc:TabStrip>
             
                    <table>
                        <tr>
                            <td>
                                <Telerik:RadLabel ID="lblUsername" runat="server" Text="User name"></Telerik:RadLabel>&nbsp;&nbsp;<Telerik:RadTextBox runat="server" ID="txtUser" CssClass="input"  OnTextChanged="txtUser_TextChanged" AutoPostBack="true"></Telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>


             <telerik:RadGrid RenderMode="Lightweight" ID="gvuserLoginAudit" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvuserLoginAudit_ItemCommand" OnSorting="gvuserLoginAudit_Sorting" OnNeedDataSource="gvuserLoginAudit_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="User Name" AllowSorting="true" SortExpression="FLDUSERNAME">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></Telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Client IP" AllowSorting="true" SortExpression="FLDCLIENTIP">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <Telerik:RadLabel ID="lblClientIP" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLIENTIP") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                          
                        </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Accessed On" >
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <Telerik:RadLabel ID="lblMachine" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCESSSEDON") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                          
                        </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Success" >
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <Telerik:RadLabel ID="lblSuccess" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUCCESSYN") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                          
                        </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Attempt Count" >
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblACount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTEMPTCOUNT") %>'></Telerik:RadLabel>
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
                               
  