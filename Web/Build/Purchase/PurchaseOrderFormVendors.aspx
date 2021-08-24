<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderFormVendors.aspx.cs" Inherits="PurchaseOrderFormVendors" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase OrderForm Vendors</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

             <table id="tblzone" width="100%">
                <tr>
                    <td >
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td >
                       <eluc:Vessel ID="UcVesselSearch" runat="server" OnTextChangedEvent="ucVessel_TextChangedEvent" AutoPostBack="true" VesselsOnly="true" Width="80%" AppendDataBoundItems="true" />

                    </td>
                    <td >
                        <telerik:RadLabel ID="lblstock" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td >
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlStockType" OnTextChanged="ddlStockType_TextChanged" AutoPostBack="true" EnableLoadOnDemand="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="Spares" Value="SPARE"/>
                            <telerik:RadComboBoxItem Text="Stores" Value="STORE" />
                            <telerik:RadComboBoxItem Text="Service" Value="SERVICE" />
                        </Items>
                    </telerik:RadComboBox>

                    </td>
              
               
                    <td >
                        <telerik:RadLabel ID="lblfromdate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td >
                        <eluc:Date ID="ucfromdatesearch" runat="server"  />

                    </td>
                    <td >
                        <telerik:RadLabel ID="lbltodate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td >
                       <eluc:Date ID="uctodatesearch" runat="server"  />

                    </td>
                      <td >
                        <telerik:RadLabel ID="lblamount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td >
                        <eluc:Decimal ID="ucdecimalamount" runat="server"  Width="60%"/>

                    </td>
                </tr>
            </table>
        

            <eluc:TabStrip ID="MenuPurchaseordervendor" runat="server" OnTabStripCommand="Purchaseordervendor_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPurchaseordervendor" Height="86%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvPurchaseordervendor_ItemCommand" OnItemDataBound="gvPurchaseordervendor_ItemDataBound" OnNeedDataSource="gvPurchaseordervendor_NeedDataSource"
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
                        <telerik:GridTemplateColumn HeaderText="PR Creation Date">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcreationdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATIONDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                       <telerik:GridTemplateColumn HeaderText="Po Date">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpodate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPODATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="PO Ref">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblporef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOREFRENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Agresso PO ID">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblaggressopoid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGGRESSOPOID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Total Items">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltotalitems" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALITEMS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Approx PoAmount In USD">
                          <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblapproxamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROXTOTALAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Main Vendor">
                           <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmainvendorname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAINVENDORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Total No Of Qtn Sent">
                           <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltotalquotationsent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALQUOTATIONSENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Total No Of Qtn Submitted">
                           <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltotalquotationsub" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOATALQUOTATIONSUMBMITTED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Qtn Vendor">
                           <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblqtnvendorname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONVENDORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Currency Code">
                           <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcurrencycode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Total Cost">
                           <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltotalcost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOST","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                         
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Approx QTN Amount In USD">
                           <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblapproxqtnamt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROXIMATEAMTINUSD","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Qtn Reason Option">
                           <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblreasonoption" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTAIONRESONOPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Qtn Reason Others">
                           <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblqtnreason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTAIONRESONOTHERS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                       </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Approved By ">
                           <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblapprovedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDBY") %>'></telerik:RadLabel>
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
