<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenancePlannedWorkOrderReport.aspx.cs"
    Inherits="PlannedMaintenancePlannedWorkOrderReport" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Parameter" Src="~/UserControls/UserControlJobParameterValue.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Report Work Order</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%= btnConfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="radSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>

            <table style="width: 99%; border: 1px; background-color: cornflowerblue; color: red; padding: 5px; font: bold">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblworkorderNo" Text="WORK ORDER NO :" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" Text="CATEGORY :" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlanDate" Text="PLAN DATE :" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDuration" Text="DURATION :" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblResponsible" Text="ASSIGNED TO :" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblStatus" Text="STATUS :" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                OnItemCommand="gvWorkOrder_ItemCommand1" EnableViewState="true" Height="80%" OnSortCommand="gvWorkOrder_SortCommand"
                OnDeleteCommand="gvWorkOrder_DeleteCommand" OnItemDataBound="gvWorkOrder_ItemDataBound1" OnPreRender="gvWorkOrder_PreRender"
                OnItemCreated="gvWorkOrder_ItemCreated" AllowMultiRowEdit="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Comp. No.">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comp. Name">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="285px" HeaderText="Job Title" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDWORKORDERNAME" DataField="FLDWORKORDERNAME" ShowFilterIcon="false" FilterDelay="200">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkorderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkorderGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERREPORTID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkWorkorderName" runat="server"
                                    Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblJobDone" runat="server" Text="Job Done"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadRadioButtonList ID="rblJobDoneStatus" runat="server" OnSelectedIndexChanged="rblJobDoneStatus_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="1" Text="Yes"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="2" Text="Defects"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="3" Text="No" Selected="True"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Parameters">
                            <ItemTemplate>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Parameter ID="ucParameter" runat="server" WorkOrderId='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></eluc:Parameter>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Done Date" HeaderStyle-Width="140px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWORKDONEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDoneDateEdit" runat="server" Width="100%" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWORKDONEDATE")) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comments" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHISTORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCommentEdit" runat="server" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHISTORY") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="cmdAtt" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
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
            <table>
            </table>
            <asp:Button ID="btnConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
