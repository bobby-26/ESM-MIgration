<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderPartsRequired.aspx.cs" Inherits="PlannedMaintenanceWorkOrderPartsRequired" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Parts Required</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                if (radwindow != null) {
                    radwindow.close();
                }
                if (typeof parent.CloseUrlModelWindow === "function") { parent.CloseUrlModelWindow(); }
                if (typeof parent.refreshPart === "function") { parent.refreshPart(); }
            }
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvUsesParts.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmgvUsesParts" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />

        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenugvUsesParts" runat="server" OnTabStripCommand="gvUsesParts_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid ID="gvUsesParts" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnItemCommand="RadGrid1_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnNeedDataSource="RadGrid1_NeedDataSource" Width="100%" OnBatchEditCommand="gvUsesParts_BatchEditCommand" 
            OnItemDataBound="RadGrid1_ItemDataBound">
            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
            <MasterTableView EditMode="Batch" CommandItemDisplay="Top" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDWORKORDERLINEITEMID,FLDWORKORDERID,FLDSPAREITEMID">
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                <HeaderStyle Width="102px" />
                 <BatchEditingSettings EditType="Cell" /> 
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn>
                        <HeaderStyle Width="30px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20px"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="imgFlag" runat="server" Visible="false" ImageUrl="<%$ PhoenixTheme:images/red.png%>" />
                            <telerik:RadLabel ID="lblminqtyflag" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.MINQTYFLAGE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Part Number" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNUMBER">
                        <HeaderStyle Width="70px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblWorkOrderLineID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERLINEITEMID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkOrderID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSpareItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblPartNumber" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Part Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                        <HeaderStyle Width="230px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPartName" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Maker's Reference">
                        <HeaderStyle Width="150px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMakerReference" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Location">
                        <ItemStyle Wrap="False"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLocation" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Quantity in Location" HeaderStyle-HorizontalAlign="Right">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLocQuantity" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONROB","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-HorizontalAlign="Right">
                        <HeaderStyle Width="50px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblQuantity" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Required" HeaderStyle-Width="60px" UniqueName="FLDQUANTITY" HeaderStyle-HorizontalAlign="Right">
                        <ItemStyle Wrap="false" HorizontalAlign="Right" Width="60px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRequiredQty" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtRequired" runat="server" MaxLength="7" Width="100%" IsInteger="true" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDQUANTITY","{0:n0}") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle Width="60px" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            
                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="ROBislessthanMinimumLevel" runat="server" Text="* ROB is less than Minimum Level"></telerik:RadLabel>
                </td>
            </tr>
        </table>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="400px" Height="300px" Modal="true" Title="Copy" VisibleStatusbar="false" OffsetElementID="main" OnClientClose="CloseWindow">
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1">
                     <eluc:Error ID="ucError2" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:TabStrip ID="menuWorkorderCopy" runat="server" OnTabStripCommand="menuWorkorderCopy_TabStripCommand" />
                    <table border="0" width="100%">
                        <tr>
                            <td width="20%">
                                <telerik:RadLabel ID="lblCopy" runat="server" Text="Copy To"></telerik:RadLabel>
                            </td>
                            <td width="80%">
                                <telerik:RadComboBox ID="ddlWorkOrder" runat="server" Width="100%">                                    
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
