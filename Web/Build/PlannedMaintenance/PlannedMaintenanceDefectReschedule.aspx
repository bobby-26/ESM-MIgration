<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDefectReschedule.aspx.cs"
    Inherits="PlannedMaintenanceDefectReschedule" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Postpone</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderRequisition" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="menuReschedule" runat="server" OnTabStripCommand="menuReschedule_TabStripCommand" />
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Planned Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDatePicker ID="txtPlannedDate" runat="server" Enabled="false" Width="120px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblpostponeDate" runat="server" Text="Postpone Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDatePicker ID="txtpostponeDate" runat="server" CssClass="input_mandatory" Width="120px" />
                    <asp:RequiredFieldValidator ID="DueDateRequiredFieldValidator" runat="server" Display="Dynamic" ForeColor="Red"
                        ControlToValidate="txtpostponeDate" ErrorMessage="Date is required">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPostponeRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="input_mandatory" Width="140px"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator
                        ID="RemarksRequiredFieldValidator"
                        runat="server"
                        Display="Dynamic"
                        ControlToValidate="txtRemarks"
                        EnableClientScript="true" ForeColor="Red"
                        ErrorMessage="Remarks is required">*
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary ID="validationsummary" runat="server" Width="200px" BorderWidth="1px" />
                </td>
            </tr>
        </table>
        <telerik:RadGrid ID="gvReschedule" runat="server" OnNeedDataSource="gvReschedule_NeedDataSource" AutoGenerateColumns="false"
            EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="76px" AllowSorting="true" ShowFilterIcon="false"
                        ShowSortIcon="true" SortExpression="FLDPOSTPONEDATE" DataField="FLDPLANNINGDUEDATE" FilterDelay="200">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPostponeDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPOSTPONEDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="FLDPOSTPONEREMARKS" HeaderText="Remarks"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FLDPOSTPONEDBY" HeaderText="Postponed By"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
