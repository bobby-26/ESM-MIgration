<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCommonSyncStatus.aspx.cs"
    Inherits="DashboardCommonSyncStatus" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Synchronization Status</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
           
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid ID="gvVesselSynchronizationStatus" runat="server" AutoGenerateColumns="false" 
                EnableViewState="false" CellSpacing="0" GridLines="None" AllowSorting="true" EnableHeaderContextMenu="true"
                Height="88%" OnNeedDataSource="gvVesselSynchronizationStatus_NeedDataSource">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Synch. Enabled" HeaderStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="FLDSYNCENABLED">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSyncEnabled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSYNCENABLED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel Name[ID]" HeaderStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Last Exported Sequence" HeaderStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="FLDEXPORTSEQUENCE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastExportedSequence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPORTSEQUENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Exported On (GMT)" HeaderStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="FLDEXPORTDATE" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExportedOn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPORTDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Last Imported Sequence" HeaderStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="FLDIMPORTSEQUENCE" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastImportSequence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTSEQUENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Imported On (GMT)" HeaderStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="FLDIMPORTDATE" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblImportedOn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTDATE") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
