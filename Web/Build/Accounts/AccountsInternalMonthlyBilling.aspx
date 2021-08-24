<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInternalMonthlyBilling.aspx.cs" Inherits="AccountsInternalMonthlyBilling" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Internal Monthly Billing</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">  
    
     <%: Scripts.Render("~/bundles/js") %>
     <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCommonBudgetGroupAllocation" runat="server" submitdisabledcontrols="true">
          <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="92%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        
                
                    <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
             
                    <eluc:TabStrip ID="MenuBudgetTab" runat="server" TabStrip="true" OnTabStripCommand="BudgetTab_TabStripCommand">
                    </eluc:TabStrip>
               
                <table id="tblBudgetGroupAllocationSearch" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucFinancialYear" runat="server" AppendDataBoundItems="true" CssClass="input"
                                QuickTypeCode="55" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucMonth" runat="server" AppendDataBoundItems="true" SortByShortName="true"
                                CssClass="input" HardTypeCode="55" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                         <td>
                            <telerik:RadLabel ID="lblChargingStatus" runat="server" Text="Charging Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox RenderMode="Lightweight" CssClass ="input" runat="server" ID="ddlChargingStatus"  AutoPostBack="true" EnableLoadOnDemand="true">
                            <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                            <telerik:RadComboBoxItem Text="Pending" Value="0" />
                            <telerik:RadComboBoxItem Text="In Process" Value="1" />
                             <telerik:RadComboBoxItem Text="Completed" Value="2"/>
                           </Items>
                            </telerik:RadComboBox>
                          <%--  <asp:DropDownList ID="ddlChargingStatus" runat ="server" AppendDataBoundItems="true"  CssClass ="input" >
                            <asp:ListItem Text ="--Select--" Value ="Dummy"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                            <asp:ListItem Text="In Process" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                    </tr>
                </table>
              
                    <eluc:TabStrip ID="MenuCommonBudgetGroupAllocation" runat="server" OnTabStripCommand="CommonBudgetGroupAllocation_TabStripCommand">
                    </eluc:TabStrip>
             
              <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvMonthlyBilling" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvMonthlyBilling" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvMonthlyBilling_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvMonthlyBilling_SelectedIndexChanging"
                    OnItemDataBound="gvMonthlyBilling_ItemDataBound" OnItemCommand="gvMonthlyBilling_ItemCommand"
                    ShowFooter="false" ShowHeader="true" OnSortCommand="gvMonthlyBilling_SortCommand" Height="94%">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDPORTAGEBILLID">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                           <%-- <HeaderTemplate>
                                <asp:LinkButton ID="lnkVesselCodeHeader" runat="server" CommandName="Sort"
                                    CommandArgument="FLDVESSELNAME" ForeColor="White">Vessel &nbsp;</asp:LinkButton>
                                <img id="FLDDESCRIPTION" runat="server" visible="false" />
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkVesselCode" runat="server"  Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                    CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex%>'></asp:LinkButton>
                                <asp:LinkButton ID="lnkAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex%>'></asp:LinkButton>    
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>                               
                                <telerik:RadLabel ID="lblPortageBillId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGEBILLID") %>'></telerik:RadLabel>                               
                                <telerik:RadLabel ID="lblInternalMonthlyBillingId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERNALMONTHLYBILLINGID") %>'></telerik:RadLabel>                               
                            </ItemTemplate>                            
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Code">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountId" runat="server"  Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Date">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Date">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                        
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblToDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Portage Bill Status">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPortageBillStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGEBILLSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charging Status">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChargingStatus" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARGINGSTATUS") %>'></telerik:RadLabel>                                
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
      </telerik:RadAjaxPanel>
    </form>
</body>
</html>
