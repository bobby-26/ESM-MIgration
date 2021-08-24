<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrydockCostIncurred.aspx.cs" Inherits="DryDock_DrydockCostIncurred" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vessellist" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cost Incurred</title>
       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
           <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvCostincurred.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
           </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <table  style="margin-left: 20px" >
            <tbody>
                 <tr>
                    <td colspan="6">
                        <br />
                    </td>
                </tr>
            <tr>
                <td>
                <telerik:RadLabel runat="server" Text="Vessel"></telerik:RadLabel>
           </td>
                <td>
                    &nbsp
                </td>
                <td >
                    <eluc:vessellist ID="ddlvessellist" runat="server" Width="250px" CssClass="input" SyncActiveVesselsOnly="true"  ManagementType="FUL" 
                      Entitytype="VSL"  AutoPostBack="false" ActiveVesselsOnly="true" VesselsOnly="true" />
                </td>
                <td>&nbsp</td>
                <td>
                   <telerik:RadLabel runat="server" ID="RadLabel1" Text="Yard"/>
                </td>
                <td>
                    <telerik:RadLabel runat="server" ID="radlblyard" />
                </td>
                
                     </tr>
                <tr>
                    <td colspan="6">
                        <br />
                    </td>
                </tr>
            <tr>
                <td>
                    <telerik:RadLabel runat="server" Text="Project"></telerik:RadLabel>
                </td>
                <td>
                    &nbsp
                </td>
                <td >
                    <telerik:RadLabel runat="server" ID="radlblproject"></telerik:RadLabel>
                </td>
                  <td>&nbsp</td>
                <td>
                   <telerik:RadLabel runat="server" ID="RadLabel2" Text="Estimated Cost (USD)"/>
                </td>
                <td>
                    <telerik:RadLabel runat="server" ID="radestcost" />
                </td>
            </tr>
            </tbody>
        </table>
         <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
         <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvCostincurred" AutoGenerateColumns="false"
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvCostincurred_NeedDataSource"  
                OnItemDataBound="gvCostincurred_ItemDataBound" OnItemCommand="gvCostincurred_ItemCommand"   >
                <MasterTableView DataKeyNames="FLDCOSTINCURREDID" AutoGenerateColumns="false"
                    EnableColumnsViewState="false" TableLayout="Fixed" 
                    ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage">
                 
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true" ></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                  
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date"  >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                               <asp:LinkButton runat="server" ID="date" Text='<%#  General.GetDateTimeToString((DataBinder.Eval(Container, "DataItem.FLDDATE").ToString()))%>'/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Incurred Cost"  >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                     <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOSTINCURRED")%>'/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remaining Cost"  >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                     <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMAININGCOST")%>'/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Projected Variance"  >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                     <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVARIANCECOST")%>'/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
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
