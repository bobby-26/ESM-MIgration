<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListEmployeeAccount.aspx.cs"
    Inherits="CommonPickListEmployeeAccount" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
     <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
     <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="form1" runat="server">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
          
        <eluc:TabStrip ID="MenuBudget" runat="server" OnTabStripCommand="MenuBudget_TabStripCommand">
        </eluc:TabStrip>
  
    <br clear="all" />
    
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
         
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSubAccountCode" runat="server" Text="Employee Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSubAccountCodeSearch" runat="server" CssClass="input" Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDescription" runat="server" Text="Employee Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtDescriptionSearch" runat="server" CssClass="input" Text=""></telerik:RadTextBox>
                        </td>
                    </tr>                                        
                </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBudget" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvBudget_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" Height="93%"
                OnItemCommand="gvBudget_ItemCommand" 
                ShowFooter="false" ShowHeader="true" OnSortCommand="gvBudget_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false"  >         
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Employee Code" AllowSorting="true" Sortexpression="FLDSUBACCOUNT">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <%--<HeaderTemplate>
                                <asp:label ID="lblBudgetHeader" runat="server" CommandName="Sort" CommandArgument="FLDSUBACCOUNT"
                                    ForeColor="White">Employee Code&nbsp;</asp:label>
                                <img id="FLDSUBACCOUNT" runat="server" visible="false" />
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblBudgetId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTMAPID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblBudgetCodeId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkBudget" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTCODE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDescriptionHeader" runat="server">Employee Name</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblDescription" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat ="server" ID="lblBudgetGroupId" Text ='<%#  DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' Visible ="false" ></telerik:RadLabel>
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
        
    <eluc:Confirm ID="ucConfirm" runat="server" Visible="false"  OnConfirmMesage="CloseWindow"  />
    </form>
</body>
</html>



       