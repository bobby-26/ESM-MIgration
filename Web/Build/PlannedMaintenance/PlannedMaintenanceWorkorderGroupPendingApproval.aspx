<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkorderGroupPendingApproval.aspx.cs"
    Inherits="PlannedMaintenanceWorkorderGroupPendingApproval" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PTW List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true" Width="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="MenuWorkOrder" runat="server"
                TabStrip="true"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkorderList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="true" Height="100%" AllowMultiRowEdit="true" Width="100%"
                OnNeedDataSource="gvJhaList_NeedDataSource" OnItemCommand="gvWorkorderList_ItemCommand" OnItemDataBound="gvWorkorderList_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Work Order No">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkorderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkorderNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkorderName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlannedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Assigned To">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAssignedTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkApprove" runat="server" CommandName="APPROVE" ToolTip="Approve">
                                    <span class="icon"><i class="fas fa-user-check"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="400px" Height="300px" Modal="true" OffsetElementID="main" OnClientClose="CloseWindow">
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1">
                    <table border="0">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input_mandatory upperCase"
                                    MaxLength="200" Width="200px" Height="100">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator
                                    ID="TitleRequiredFieldValidator"
                                    runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="txtRemarks"
                                    EnableClientScript="true" ForeColor="Red"
                                    ErrorMessage="Remarks is required"
                                    ValidationGroup="WorkOrderDetail">*
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="center">
                                <telerik:RadButton ID="btnApprove" Text="Approve" runat="server" OnClick="btnApprove_Click"></telerik:RadButton>
                                <telerik:RadButton ID="btnReject" Text="Reject" runat="server" OnClick="btnReject_Click" ValidationGroup="WorkOrderDetail"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="200px"
                                    BorderWidth="1px" HeaderText="List of errors"></asp:ValidationSummary>
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
