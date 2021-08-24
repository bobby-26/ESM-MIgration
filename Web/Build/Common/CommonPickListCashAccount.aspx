<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListCashAccount.aspx.cs"
    Inherits="CommonPickListCashAccount" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Account List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <%: Scripts.Render("~/bundles/js") %>
      <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuAccount" runat="server" OnTabStripCommand="MenuAccount_TabStripCommand">
        </eluc:TabStrip>
  
    <br clear="all" />
     <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
         
                <table>
                    <tr>
                        <td width="5%">
                            <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                        </td>
                        <td width="5%">
                            <telerik:RadTextBox ID="txtAccountCodeSearch" CssClass="input" runat="server" Text="" MaxLength="100"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td width="5%">
                            <telerik:RadLabel ID="lblAccountDescription" runat="server" Text="Account Description"></telerik:RadLabel>
                        </td>
                        <td width="5%">
                            <telerik:RadTextBox ID="txtAccountDescSearch" CssClass="input" runat="server" Text="" MaxLength="100"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>

           <telerik:RadGrid RenderMode="Lightweight" ID="gvAccount" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvAccount_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" Height="93%" 
               OnItemCommand="gvAccount_ItemCommand" 
                ShowFooter="false" ShowHeader="true" OnSortCommand="gvAccount_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Account Code" AllowSorting="true" SortExpression="FLDACCOUNTCODE">
                              <HeaderStyle Width="102px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                         <%--   <HeaderTemplate>
                                <asp:LinkButton ID="lblAccountCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDACCOUNTCODE"
                                    ForeColor="White">Account Code&nbsp;</asp:LinkButton>
                                <img id="FLDACCOUNTCODE" runat="server" visible="false" />
                              
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAccountId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkAccountCode" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn>
                              <HeaderStyle Width="200px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAccountDescriptionHeader" runat="server">Account Description</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAccountDescription" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                              <HeaderStyle Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAccountTypeHeader" runat="server">Account Type</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTTYPE") %>'></telerik:RadLabel>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

    
