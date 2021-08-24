<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCompanyCombinedTrailBalance.aspx.cs"
    Inherits="Accounts_AccountsCompanyCombinedTrailBalance" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title>Combined Trial Balance</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

             <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server" autocomplete="off">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
         <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1" ></telerik:RadWindowManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />

   <%-- <eluc:Confirm ID="ucConfirmMessage" runat="server" OnConfirmMesage="OnAction_Click"
        OKText="Yes" CancelText="No" />--%>
  
          <asp:Button ID="ucConfirm" runat="server" Text="confirmation message" OnClick="OnAction_Click" cssclass="hidden"/>
         
            <asp:Button runat="server" ID="cmdHiddenSubmit" cssclass="hidden"/>
    
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" TabStrip="false" OnTabStripCommand="MenuReportsFilter_TabStripCommand">
            </eluc:TabStrip>
      
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadAjaxPanel ID="pnlcompanyList" runat="server" ScrollBars="Auto" Height="250px" BorderWidth="1px"
                            Width="500px">
                            <asp:CheckBoxList ID="chkCompanyList" runat="server">
                            </asp:CheckBoxList>
                        </telerik:RadAjaxPanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="As On Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccurateDate" runat="server" Text="Report is Accurate as on"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAccurateDatedet" runat="server" ></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                <td colspan="2">
                 <asp:ImageButton ID="imgExcel" runat="server"  ImageUrl="~/css/Theme1/images/xls.png"
                      OnClick ="imgExcel_OnClientClick" Width="20px" />
                </td>
                </tr>
            </table>

         <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvVender" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVender" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"  AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" 
                    ShowFooter="false" ShowHeader="true"  OnItemDataBound="gvVender_ItemDataBound">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>  
             
                </telerik:RadGrid>

         <%--   <asp:GridView ID="gvVender" runat="server" AutoGenerateColumns="true" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound ="gvVender_RowDataBound" EnableViewState="false" ShowFooter="true" >
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="false" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" Wrap="false"/>
                <Columns >
                </Columns>
            </asp:GridView>--%>
    
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvExcludedVouchers" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvExcludedVouchers" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" AllowMultiRowSelection="true" FilterType="CheckList"  OnNeedDataSource="gvExcludedVouchers_NeedDataSource" 
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" 
                    ShowFooter="false" ShowHeader="true" >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >
      <%--  <asp:GridView ID="gvExcludedVouchers" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="true" ShowHeader="true"
            EnableViewState="false" AllowSorting="true">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <Columns>
                <telerik:GridTemplateColumn HeaderText="Company">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                  
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYCODE") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Date">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Number">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                  
                  <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText="Voucher Status">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Created Date/By">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                   
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucher" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDET") %>'></telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Voucher Update Date/By">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="lblVoucherStatuss" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTUPDATEDET") %>'></telerik:RadLabel>
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
      </form>
</body>
</html>
    
  