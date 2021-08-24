<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockJobProgress.aspx.cs" Inherits="DryDock_DryDockJobProgress" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dry Dock Job Progress</title>
     <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

          <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvprogress.ClientID %>"));
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
    
        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" >
        <%-- For Popup Relaod --%>
          <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <table>
            <tbody>
                <tr>
                    <td>
                        Date
                    </td>
                    <td>&nbsp</td>
                    <td>
                        <eluc:Date runat="server" ID="radtbdate"  CssClass="input_mandatory"/>
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        Yard
                    </td>
                    <td>&nbsp</td>
                    <td>
                        <telerik:RadLabel ID="lblyard" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
         <eluc:TabStrip ID="gvmenu" runat="server" OnTabStripCommand="gvmenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
         <telerik:RadGrid RenderMode="Lightweight" ID="gvprogress"  runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvprogress_NeedDataSource"  OnBatchEditCommand="gvprogress_BatchEditCommand" 
            OnItemDataBound="gvprogress_ItemDataBound" AllowAutomaticInserts="True"
                 OnItemCommand="gvprogress_ItemCommand"
             >
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="Batch" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDORDERLINEID" CommandItemDisplay="Top" CommandItemSettings-ShowAddNewRecordButton="false" >
                <BatchEditingSettings EditType="Cell" HighlightDeletedRows="true"/>
                <HeaderStyle HorizontalAlign="Center" />
                
               
                <Columns>
                    <telerik:GridTemplateColumn HeaderText='id' AllowSorting='true' Visible="false" UniqueName="FLDORDERLINEID" HeaderStyle-Width="0%">
                       <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            
                            <telerik:RadLabel ID="lblid"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText='Number' AllowSorting='true' HeaderStyle-Width="10%" >
                       <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            
                            <telerik:RadLabel ID="lblnumber"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Name' AllowSorting='true' >
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText='Total Cost (USD)' AllowSorting='true' >
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbltotalcost" runat="server" Text='<%#String.Format("{0:f2}", DataBinder.Eval(Container,"DataItem.FLDTOTALCOSTINUSD")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                       <telerik:GridTemplateColumn HeaderText='Progress(%)' AllowSorting='true' UniqueName="FLDPROGRESS" >
                           <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblprogress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROGRESS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                           <EditItemTemplate>
<%--                               <telerik:RadNumericTextBox RenderMode="Lightweight" Width="55px" runat="server" ID="tbunitprogress"/>--%>
                               <eluc:Decimal runat="server" ID="tbunitprogress" MaxValue="100" MinValue="0" />
                           </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                   <%-- <telerik:GridTemplateColumn HeaderText='Cost Incurred' AllowSorting='true' >
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcostincurred" runat="server" Text='<%# String.Format("{0:f2}",DataBinder.Eval(Container.DataItem, "FLDCOSTINCURRED"))%>
'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                     <telerik:GridTemplateColumn HeaderText='Cost Incurred (USD)' AllowSorting='true' >
                         <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcostusd" runat="server" Text='<%# String.Format("{0:f2}",DataBinder.Eval(Container.DataItem, "FLDCOSTINCURREDINUSD"))%>'></telerik:RadLabel>
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
