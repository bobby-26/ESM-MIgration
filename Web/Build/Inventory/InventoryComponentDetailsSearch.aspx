<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentDetailsSearch.aspx.cs" Inherits="InventoryComponentDetailsSearch" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component</title>
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %> 

        <script type="text/javascript">
            var DWPgrid = null;
            function PaneResized(sender, args) {                
                var $ = $telerik.$;
                var height = $(window).height();                
                if (DWPgrid != null && DWPgrid.GridDataDiv != null) {
                    var gridPagerHeight = (DWPgrid.PagerControl) ? DWPgrid.PagerControl.offsetHeight : 0;
                    DWPgrid.GridDataDiv.style.height = (height - gridPagerHeight - 100) + "px";
                } else {
                                       
                    var gvComponents = $find("<%= gvComponents.ClientID %>");
                    var gvWorkOrder = $find("<%= gvWorkOrder.ClientID %>");
                    var gvSpare = $find("<%= gvSpare.ClientID %>");
                    var gvComponentsPagerHeight = (gvComponents.PagerControl) ? gvComponents.PagerControl.offsetHeight : 0;
                    var gvWorkOrderPagerHeight = (gvWorkOrder.PagerControl) ? gvWorkOrder.PagerControl.offsetHeight : 0;
                    var gvSparePagerHeight = (gvSpare.PagerControl) ? gvSpare.PagerControl.offsetHeight : 0;

                    gvComponents.GridDataDiv.style.height = (Math.round(height / 3) - gvComponentsPagerHeight - 69) + "px";
                    gvWorkOrder.GridDataDiv.style.height = (Math.round(height / 3) - gvWorkOrderPagerHeight - 69) + "px";
                    gvSpare.GridDataDiv.style.height = (Math.round(height / 3) - gvSparePagerHeight - 69) + "px";
                }
            }
            function pageLoad() {
                if (top.componentexpandcollapse != null) {
                    expandcollapse(top.componentexpandcollapse);                    
                } else {
                    PaneResized();
                }
            }
            function expandcollapse(gridid) {
                top.componentexpandcollapse = gridid;
                var $ = $telerik.$;
                var height = $(window).height();
                var atab = document.querySelector('#MenuWorkOrder_dlstTabs');
                var btab = document.querySelector('#MenuJobs_dlstTabs');
                var ctab = document.querySelector('#MenuSpare_dlstTabs');

                var gvPlanned = $find("<%= gvComponents.ClientID %>");
                var gvProgress = $find("<%= gvWorkOrder.ClientID %>");
                var gvCompleted = $find("<%= gvSpare.ClientID %>");
                DWPgrid = $find(gridid);
                var collapse = false;
                if (DWPgrid != gvPlanned) {
                    var visible = gvPlanned.get_visible();
                    gvPlanned.set_visible(!visible);
                    atab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (DWPgrid != gvProgress) {
                    var visible = gvProgress.get_visible();
                    gvProgress.set_visible(!visible);
                    btab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (DWPgrid != gvCompleted) {
                    var visible = gvCompleted.get_visible();
                    gvCompleted.set_visible(!visible);
                    ctab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (!collapse) {                    
                    DWPgrid = null;
                    top.componentexpandcollapse = null;
                }
                PaneResized();
            }           
    </script>
         </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvComponents">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID ="gvComponents" />
                        <telerik:AjaxUpdatedControl ControlID ="gvWorkOrder" />
                        <telerik:AjaxUpdatedControl ControlID ="gvSpare" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvWorkOrder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID ="gvWorkOrder" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvSpare">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID ="gvSpare" />
                        <telerik:AjaxUpdatedControl ControlID ="gvComponents" />
                        <telerik:AjaxUpdatedControl ControlID ="gvWorkOrder" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid ID="gvComponents" RenderMode="Lightweight" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemDataBound="gvComponents_ItemDataBound"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvComponents_NeedDataSource" AllowMultiRowSelection="true" OnSortCommand="gvComponents_SortCommand"
            OnItemCommand="gvComponents_ItemCommand" EnableViewState="true">

            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDCOMPONENTID" ClientDataKeyNames="FLDCOMPONENTID">
                <Columns>
                    <telerik:GridBoundColumn UniqueName="FLDCOMPONENTNUMER" HeaderText="Number" DataField="FLDCOMPONENTNUMBER" AllowSorting="true" SortExpression="FLDCOMPONENTNUMBER" DataType="System.String">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Width="90px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDCOMPONENTNAME" HeaderText="Name" DataField="FLDCOMPONENTNAME" AllowSorting="true" SortExpression="FLDCOMPONENTNAME" DataType="System.String">
                        <HeaderStyle Width="240px" />
                        <ItemStyle Width="240px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn UniqueName="FLDCOMPONENTNAME" HeaderText="Name" AllowSorting="true" SortExpression="FLDCOMPONENTNAME">
                        <HeaderStyle Width="240px" />
                        <ItemStyle Width="240px" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCompName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>' ></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="FLDTYPE" HeaderText="Type" DataField="FLDTYPE" AllowSorting="true" SortExpression="FLDTYPE" DataType="System.String">
                        <HeaderStyle Width="120px" />
                        <ItemStyle Width="120px"  />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDCATEGORYNAME" HeaderText="Category" DataField="FLDCATEGORYNAME" AllowSorting="true" SortExpression="FLDCATEGORYNAME" DataType="System.String">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Width="180px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDSERIALNUMBER" HeaderText="Serial No." DataField="FLDSERIALNUMBER" AllowSorting="true" SortExpression="FLDSERIALNUMBER" DataType="System.String">
                        <HeaderStyle Width="120px" />
                        <ItemStyle Width="120px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDCOMPONENTSTATUSNAME" HeaderText="Status" DataField="FLDCOMPONENTSTATUSNAME" AllowSorting="false" DataType="System.String">
                        <HeaderStyle Width="120px" />
                        <ItemStyle Width="120px" />
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn UniqueName="FLDCLASSCODE" HeaderText="Class Code" DataField="FLDCLASSCODE" AllowSorting="true" SortExpression="FLDCLASSCODE" DataType="System.String">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Width="90px" />
                    </telerik:GridBoundColumn>

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
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true"  EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true"  />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing  AllowColumnResize="true" />
                <ClientEvents />
            </ClientSettings>
        </telerik:RadGrid>

        <br clear="all" />
        <eluc:TabStrip ID="MenuJobs" runat="server" OnTabStripCommand="MenuJobs_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvWorkOrder" RenderMode="Lightweight" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemDataBound="gvWorkOrder_ItemDataBound"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" OnSortCommand="gvWorkOrder_SortCommand"
             EnableViewState="true">

            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID,FLDCOMPONENTID,FLDCOMPONENTJOBID">
                <Columns>
                    <telerik:GridBoundColumn UniqueName="FLDCOMPONENTNUMER" HeaderText="Number" DataField="FLDCOMPONENTNUMBER" AllowSorting="true" SortExpression="FLDCOMPONENTNUMBER" DataType="System.String">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Width="90px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDCOMPONENTNAME" HeaderText="Name" DataField="FLDCOMPONENTNAME" AllowSorting="true" SortExpression="FLDCOMPONENTNAME" DataType="System.String">
                        <HeaderStyle Width="240px" />
                        <ItemStyle Width="240px" />
                    </telerik:GridBoundColumn>
                    <%--<telerik:GridBoundColumn UniqueName="FLDWORKORDERNAME" HeaderText="Job Code & Title" DataField="FLDWORKORDERNAME" AllowSorting="true" SortExpression="FLDWORKORDERNAME" DataType="System.String">
                        <HeaderStyle Width="240px" />
                        <ItemStyle Width="240px"  />
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridTemplateColumn UniqueName="FLDWORKORDERNAME" HeaderText="Job Code & Title"  AllowSorting="true" SortExpression="FLDWORKORDERNAME"> 
                        <HeaderStyle Width="240px" />
                        <ItemStyle Width="240px"  />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblComponentJobID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblComponentID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkTitle" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="FLDJOBCATEGORY" HeaderText="Category" DataField="FLDJOBCATEGORY" AllowSorting="true" SortExpression="FLDJOBCATEGORY" DataType="System.String">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Width="180px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDFREQUENCYNAME" HeaderText="Frequency" DataField="FLDFREQUENCYNAME" AllowSorting="false" DataType="System.String">
                        <HeaderStyle Width="120px" />
                        <ItemStyle Width="120px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDDISCIPLINENAME" HeaderText="Responsibility" DataField="FLDDISCIPLINENAME" AllowSorting="true" SortExpression="FLDDISCIPLINENAME" DataType="System.String">
                        <HeaderStyle Width="120px" />
                        <ItemStyle Width="120px" />
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn UniqueName="FLDDUEDATE" HeaderText="Due Date" DataField="FLDDUEDATE" AllowSorting="true" SortExpression="FLDDUEDATE" DataFormatString="{0:dd/MM/yyyy}" DataType="System.DateTime">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Width="90px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDLASTDONEDATE" HeaderText="Last Done On" DataField="FLDLASTDONEDATE" AllowSorting="false" DataFormatString="{0:dd/MM/yyyy}" DataType="System.DateTime">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Width="90px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDWORKORDERSTATUS" HeaderText="Status" DataField="FLDWORKORDERSTATUS" AllowSorting="false" DataType="System.String">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDWORKORDERGROUPNO" HeaderText="Workorder" DataField="FLDWORKORDERGROUPNO" AllowSorting="false" DataType="System.String">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Width="90px" />
                    </telerik:GridBoundColumn>

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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing  AllowColumnResize="true" />
                <ClientEvents />
            </ClientSettings>
        </telerik:RadGrid>

        <br />
        <eluc:TabStrip ID="MenuSpare" runat="server" OnTabStripCommand="MenuSpare_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvSpare" RenderMode="Lightweight" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemDataBound="gvSpare_ItemDataBound"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvSpare_NeedDataSource" AllowMultiRowSelection="true" OnSortCommand="gvSpare_SortCommand"
             OnItemCommand="gvSpare_ItemCommand" EnableViewState="true">

            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDSPAREITEMID" ClientDataKeyNames="FLDSPAREITEMID">
                <Columns>
                    <telerik:GridBoundColumn UniqueName="FLDCOMPONENTNAME" HeaderText="Component" DataField="FLDCOMPONENTNAME" AllowSorting="true" SortExpression="FLDCOMPONENTNAME" DataType="System.String">
                        <HeaderStyle Width="240px" />
                        <ItemStyle Width="240px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDNUMBER" HeaderText="Name" DataField="FLDNUMBER" AllowSorting="true" SortExpression="FLDNUMBER" DataType="System.String">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Width="1000px"  />
                    </telerik:GridBoundColumn>
                    <%--<telerik:GridBoundColumn UniqueName="FLDNAME" HeaderText="Name" DataField="FLDNAME" AllowSorting="true" SortExpression="FLDNAME" DataType="System.String">
                        <HeaderStyle Width="240px" />
                        <ItemStyle Width="240px"  />
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridTemplateColumn UniqueName="FLDNAME" HeaderText="Name" AllowSorting="true" SortExpression="FLDNAME">
                        <HeaderStyle Width="240px" />
                        <ItemStyle Width="240px" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lblName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="FLDMAKERNAME" HeaderText="Maker" DataField="FLDMAKERNAME" AllowSorting="false" DataType="System.String">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Width="180px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDUNITNAME" HeaderText="Unit" DataField="FLDUNITNAME" AllowSorting="false" DataType="System.String">
                        <HeaderStyle Width="40px" />
                        <ItemStyle Width="40px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDSPAREMINIMUM" HeaderText="Min Qtn" DataField="FLDSPAREMINIMUM" AllowFiltering="false" DataType="System.Int16">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="6px" HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDREORDERLEVEL" HeaderText="Reorder Level" DataField="FLDREORDERLEVEL" DataType="System.Int16">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="6px" HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="FLDROB" HeaderText="ROB" DataField="FLDROB" AllowSorting="false" DataType="System.Int32">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="6px" HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing  AllowColumnResize="true" />
                <ClientEvents />
            </ClientSettings>
        </telerik:RadGrid>
        
    </form>
</body>
</html>
