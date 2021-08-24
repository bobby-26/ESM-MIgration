<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDirectNCDefectJobAdd.aspx.cs" Inherits="InspectionDirectNCDefectJobAdd" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Component" Src="~/UserControls/UserControlMultiColumnComponents.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmdefectjobadd" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuSave" runat="server" OnTabStripCommand="MenuSave_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="3" cellspacing="3" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListComponent">
                        <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                            Enabled="false" Width="60px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="20"
                            Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                        <asp:LinkButton ID="imgComponent" runat="server">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtComponentId" runat="server" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lbldetailsofthedefect" runat="server" Text="Defect Details"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtdetailsofthedefect" runat="server" CssClass="gridinput_mandatory"
                        TextMode="Multiline" Resize="Both" Width="300px" Rows="8">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblduedate" runat="server" Text="Due Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDueDate" runat="server" CssClass="gridinput_mandatory" Width="150px"></eluc:Date>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblResponsibilitysearch" runat="server" Text="Responsibility"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Discipline ID="ucDisciplineResponsibility" runat="server" AppendDataBoundItems="true" Width="200px" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>

                    <p style="color: darkblue">
                        Note:<br />
                        Fields highlighted in "Red" color are mandatory.
                    </p>
                </td>
            </tr>
        </table>
        <telerik:RadGrid ID="gvDefectJob" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvDefectJob_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn HeaderText="Defect No." DataField="FLDDEFECTNO" HeaderStyle-Width="125px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Component No." DataField="FLDCOMPONENTNUMBER" HeaderStyle-Width="125px"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Due" HeaderStyle-Width="125px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="txtDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE"))%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="Details" DataField="FLDDETAILS"></telerik:GridBoundColumn>
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
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
