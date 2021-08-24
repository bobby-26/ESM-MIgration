<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVesselTypeSearch.aspx.cs" Inherits="Inspection_InspectionVesselTypeSearch" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Type List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    
    <form id="form2" runat="server" >
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
     <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
    <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvvesseltypelist" AutoGenerateColumns="false" Height="100%"
        AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvvesseltypelist_NeedDataSource">
        <MasterTableView EditMode="InPlace" DataKeyNames="FLDAPPLIESTO" AutoGenerateColumns="false" EnableColumnsViewState ="false"
            TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true"
            InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="false">
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
                <telerik:GridTemplateColumn HeaderText="Vessel Type">
                    <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="Radlblvesselname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAPPLIESTO")%>'>
                        </telerik:RadLabel>
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
