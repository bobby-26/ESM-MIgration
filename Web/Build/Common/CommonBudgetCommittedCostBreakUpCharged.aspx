<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetCommittedCostBreakUpCharged.aspx.cs" Inherits="CommonBudgetCommittedCostBreakUpCharged" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Committed Cost Breakup</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
    
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCostBreakup" runat="server" submitdisabledcontrols="true">

         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPeriod" runat="server" Text="Period"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPeriodName" runat="server" CssClass="readonlytextbox" ReadOnly="true" ></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <b><telerik:RadLabel ID="lblCharged" runat="server" Text="Charged"></telerik:RadLabel></b>
               <eluc:TabStrip ID="MenuCostBreakup" runat="server" OnTabStripCommand="CostBreakup_TabStripCommand">
                    </eluc:TabStrip>
             
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCostBreakup" Height="80%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCostBreakup_ItemCommand"  OnNeedDataSource="gvCostBreakup_NeedDataSource"
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
     
                   <%-- <asp:GridView ID="gvCostBreakup" runat="server" AutoGenerateColumns="False" Font-Size="11px" 
                        Width="100%" CellPadding="3" AllowSorting="false" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                   --%>     
                            <telerik:GridTemplateColumn Visible="false" HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lnkVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>                                
                            </telerik:GridTemplateColumn>  
                            
                            <telerik:GridTemplateColumn HeaderText="Voucher Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                                     <ItemTemplate>
                                    <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>                                
                            </telerik:GridTemplateColumn>
                                                    
                            <telerik:GridTemplateColumn HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>       
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               <ItemTemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEPOAMOUNTUSD") %>'></telerik:RadLabel>
                                </ItemTemplate>                                
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Voucher Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblApprovalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFAPPROVAL", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>                                
                            </telerik:GridTemplateColumn>
                             
                            <telerik:GridTemplateColumn HeaderText="PO Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>                          
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Over/Under Commitment">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblOverOrUnderCommit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERORUNDERCOMMITMENT") %>'></asp:Label>
                                </ItemTemplate>                                
                            </telerik:GridTemplateColumn>
                     </Columns>
                   <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>

                               
    </form>
</body>
</html>
