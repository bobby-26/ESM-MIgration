<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCMap.aspx.cs" Inherits="Dashboard_DashboardBSCMap" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
          <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvPI,table1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
           <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    
    <table border="0" width="100%" id="table1">
        <tr >
            <th align="center">
                <telerik:RadLabel runat="server"   ID="radlblstrategyname"/>
            </th>
        </tr>
        <tr>
            <td align="center">
                <telerik:RadLabel runat="server" ID="radlblstrategyvision"></telerik:RadLabel>
            </td>
        </tr>
    </table>
        <br />
                     <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

         <telerik:RadGrid runat="server" ID="gvBSCMap" AutoGenerateColumns="false" Height="67.5%"
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvBSCMap_NeedDataSource" EnableViewState="true"
                OnItemDataBound="gvBSCMap_ItemDataBound" OnItemCommand="gvBSCMap_ItemCommand" ShowFooter="false" >
                <MasterTableView EditMode="InPlace" AutoGenerateColumns="false"
                    TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="true" DataKeyNames="FLDBSSTRATEGICPERSPECTIVEID" >
                    
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns></Columns>


                   
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder"  >
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" CellSelectionMode="SingleCell" />
                   
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
             <telerik:RadContextMenu runat="server" ID="ContextMenu1"
        EnableRoundedCorners="true" EnableShadows="true"
         OnItemClick="ContextMenu1_ItemClick" >
        <Targets>
           <telerik:ContextMenuControlTarget ControlID="gvBSCMap" />
        </Targets>
        <Items>
            <telerik:RadMenuItem Text="Add/Edit Cell" Value="addkpi"/>
                   
        </Items>
    </telerik:RadContextMenu>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
