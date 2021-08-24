<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionDepartureReportReview.aspx.cs" Inherits="VesselPositionDepartureReportReview" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Review</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlPlanReliever" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
         <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
        <eluc:TabStrip ID="Review" runat="server" OnTabStripCommand="Review_TabStripCommand"></eluc:TabStrip>
        <br />
        <table width="100%">
            <tr>
                <td style="text-align-last:left"><telerik:RadLabel ID="lblvesselname" runat="server"></telerik:RadLabel></td>
                <td style="text-align-last:center"><telerik:RadLabel ID="lblReportType" Text="Departure Report" runat="server"></telerik:RadLabel></td>
                <td style="text-align-last:right"><telerik:RadLabel ID="lblDate" runat="server"></telerik:RadLabel></td>
            </tr>
        </table>
        <br />
        <telerik:RadAjaxPanel runat="server" ID="pnlPlanReliever">
            <telerik:RadGrid ID="gvParameter" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" ShowFooter="false" EnableViewState="false"
                EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemDataBound="gvParameter_ItemDataBound"
                OnNeedDataSource="gvParameter_NeedDataSource">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Normal Range" Name="Range" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="17%">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Parameter" HeaderStyle-Width="40%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblParameterName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARAMETERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reported Value" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblreportedvalue" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTEDVALUE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min" ColumnGroupName="Range" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmin" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMIN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max" ColumnGroupName="Range" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmax" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAX")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table width="60%">
                <tr>
                    <td colspan="2"><h3><telerik:RadLabel ID="lblhead" runat="server" Text="Other Items"></telerik:RadLabel></h3></td>                    
                </tr>
                <tr>
                    <td style="width:40%"><telerik:RadLabel ID="lblIcingOnDeck" runat="server" Text="Icing On Deck"></telerik:RadLabel></td>
                    <td style="width:20%; text-align:center"><telerik:RadLabel ID="lblisicing" runat="server"></telerik:RadLabel></td>
                </tr>
                <tr>
                    <td><telerik:RadLabel ID="lblinuswaters" runat="server" Text="In US Waters"></telerik:RadLabel></td>
                    <td style="text-align:center"><telerik:RadLabel ID="lblisinuswater" runat="server"></telerik:RadLabel></td>
                </tr>
                <tr>
                    <td><telerik:RadLabel ID="lblInHighRiskArea" runat="server" Text="In High Risk Area"></telerik:RadLabel></td>
                    <td style="text-align:center"><telerik:RadLabel ID="lblisInHighRiskArea" runat="server"></telerik:RadLabel></td>
                </tr>
            </table>
            <br /><br />
            <table>
                <tr>
                    <td><telerik:RadLabel ID="lblmasterremark" runat="server" Text="Master's Remarks : "></telerik:RadLabel></td>
                    <td><telerik:RadLabel ID="lblmasterremarktext" runat="server"></telerik:RadLabel></td>
                </tr>
            </table>
             <br /><br />
            <h3><telerik:RadLabel ID="lblalert" Visible="false" runat="server" Text="Alert Msg : "></telerik:RadLabel></h3>
            <telerik:RadLabel ID="lblAertText" runat="server"></telerik:RadLabel>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
