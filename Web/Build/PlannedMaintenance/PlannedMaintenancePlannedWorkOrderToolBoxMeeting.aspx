<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenancePlannedWorkOrderToolBoxMeeting.aspx.cs"
    Inherits="PlannedMaintenancePlannedWorkOrderToolBoxMeeting" %>

<%@ Import Namespace="System.Data" %>
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
    <title>Toolbox Meeting</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script>
            function showModal() {
                console.log("test");

                var path = "<%=Session["sitepath"]%>";
                openNewWindow('NAFA', '', path + "/PlannedMaintenance/PlannedMaintenancesWorkOrderDefectReport.aspx?WorkOrderId=");
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <%--            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="gvWorkOrder">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gvWorkOrder" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="MenuDivWorkOrder">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="divWorKorder" />
                            <telerik:AjaxUpdatedControl ControlID="rgvForm" />
                            <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                            <telerik:AjaxUpdatedControl ControlID="ucError" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>

            <table id="divWorKorder" style="width: 100%; border: 1px; background-color: cornflowerblue; color: red; padding: 5px; font: bold">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblworkorderID" Text="WORK ORDER NO :" runat="server" ForeColor="White" Font-Bold="true"></telerik:RadLabel>
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="285px" HeaderText="Job Title" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDWORKORDERNAME" DataField="FLDWORKORDERNAME" ShowFilterIcon="false" FilterDelay="200">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkorderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkorderGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERREPORTID") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="Select"
                                    Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Done">
                            <ItemTemplate>
                                <telerik:RadRadioButtonList ID="rblJobDoneStatus" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="rblJobDoneStatus_SelectedIndexChanged1">
                                    <Items>
                                        <telerik:ButtonListItem Value="1" Text="Yes" Selected="True"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="2" Text="Defects"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="3" Text="No"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Template" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTemplates" runat="server" Text="Download" CommandName="TEMPLATE"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Running Hrs">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRunHrs" runat="server"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Part Used" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPartUsed" runat="server" Text="Show"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Done Date" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWORKDONEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDoneDateEdit" runat="server" Text='<%# DateTime.Today.Date %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comments" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHISTORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCommentEdit" runat="server" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHISTORY") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="ATTACHEMENT" ID="cmdAtt"
                                    ToolTip="Attachment"><span class="icon"><i class="fas fa-paperclip"></i></span></asp:LinkButton>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
