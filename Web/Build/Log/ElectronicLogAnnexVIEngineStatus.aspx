<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectronicLogAnnexVIEngineStatus.aspx.cs" Inherits="Log_ElectronicLogAnnexVIEngineStatus" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Record book of Diesel Engine Status</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvEngineStatus.ClientID %>"));
                    
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
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadDropDownList runat="server" AutoPostBack="true" ID="ddlStatus" OnItemSelected="ddlStatus_ItemSelected"></telerik:RadDropDownList>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="Tabstrip" runat="server" OnTabStripCommand="Tabstrip_TabStripCommand" />

             <telerik:RadGrid runat="server" ID="gvEngineStatus" AutoGenerateColumns="false" 
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvEngineStatus_NeedDataSource" EnableViewState="true"
                OnItemDataBound="gvEngineStatus_ItemDataBound" OnItemCommand="gvEngineStatus_ItemCommand" ShowFooter="false" >
                <MasterTableView EditMode="InPlace" AutoGenerateColumns="false"
                    TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="DES" HeaderText="Diesel Engine Tier On and Off Status" HeaderStyle-HorizontalAlign="Left" >
                            
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="DE" HeaderText="Diesel Engine" HeaderStyle-HorizontalAlign="Center"  ParentGroupName="DES"/>
                        <telerik:GridColumnGroup Name="ME" HeaderText="Main Engine" HeaderStyle-HorizontalAlign="Center"  ParentGroupName="DE"/>
                        <telerik:GridColumnGroup Name="AE1" HeaderText="A/E #1" HeaderStyle-HorizontalAlign="Center"  ParentGroupName="DE"/>
                         <telerik:GridColumnGroup Name="AE2" HeaderText="A/E #2" HeaderStyle-HorizontalAlign="Center"  ParentGroupName="DE"/>
                         <telerik:GridColumnGroup Name="AE3" HeaderText="A/E #3" HeaderStyle-HorizontalAlign="Center"  ParentGroupName="DE"/>
                         <telerik:GridColumnGroup Name="HG" HeaderText="Harbour Gen" HeaderStyle-HorizontalAlign="Center"  ParentGroupName="DE"/>
                    </ColumnGroups>
                    
                    <Columns>
                        <telerik:GridTemplateColumn ColumnGroupName="DES">
                            <ItemTemplate>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="DES" HeaderText="Status Change" >
                            <HeaderStyle HorizontalAlign ="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="DES" HeaderText="Date" >
                            <HeaderStyle HorizontalAlign ="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="DES" HeaderText="Time" >
                            <HeaderStyle HorizontalAlign ="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>


                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
             </telerik:RadAjaxPanel> 
    </form>
</body>
</html>
