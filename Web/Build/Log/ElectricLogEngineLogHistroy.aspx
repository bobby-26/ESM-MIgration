<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogEngineLogHistroy.aspx.cs" Inherits="Log_ElectricLogEngineLogHistroy" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cheif Engineer's Log History</title>
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
	<script type="text/javascript">
           function Resize() {
                   TelerikGridResize($find("<%= gvEngineLogHistory.ClientID %>"));
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
    <div>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
        <eluc:Status ID="ucStatus" runat="server" Visible="false"></eluc:Status>
        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvEngineLogHistory" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvEngineLogHistory_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <ExportSettings>
                <Pdf PageTitle="Engine Log History" PaperSize="A4" />
            </ExportSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>

                    <telerik:GridTemplateColumn HeaderText='Watch' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblWATCHID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='UserId' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblUsercode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='RankId' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSIGNEDRANKID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='VAR' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPARAMETER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARAMETER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Amended' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblISAMEND" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISAMEND") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='DateTime' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDATE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Orig Value' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblORGINDALVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGINDALVALUE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Mod Value' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMODIFIEDVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDVALUE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Reason' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblREASON" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></telerik:RadLabel>
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
               <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </div>
    </form>
</body>
</html>
