<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceRiskAssessmentPendingApproval.aspx.cs" Inherits="PlannedMaintenanceRiskAssessmentPendingApproval" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Postpone Approval</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style type="text/css">
            .rbButton {
                padding: 8px 10px;
            }
        </style>
        <script type="text/javascript">

            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPostponedApproval.ClientID %>"));
                }, 200);
            }

            function pageLoad() {
                Resize();
            }
            window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmPostponedApproval" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="ajxPanel" runat="server" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click1" />
            <table width="70%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblvesselname" runat="server" Visible="false" CssClass="readonly_textbox"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblorder" runat="server" Text="WO Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtnumber" runat="server" CssClass="input" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuPostponedApproval" runat="server" OnTabStripCommand="MenuPostponedApproval_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvPostponedApproval" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" OnItemCommand="gvPostponedApproval_ItemCommand" OnItemDataBound="gvPostponedApproval_ItemDataBound1"
                OnNeedDataSource="gvPostponedApproval_NeedDataSource" EnableViewState="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblvesselname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref. No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRAid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTMACHINERYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRANumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prepared">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDPURPOSE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server">Component Name</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstatusid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsCreatedByOffice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISCREATEDBYOFFICE")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblworkordernumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblworkorderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Priority">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Intended Work">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINTENDEDWORKDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtstatus" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDSTATUSNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDREJECTEDBYNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblReasonDateHeader" runat="server">Rejected Date</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREJECTEDDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rejected Reason" Visible="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDREMARKS"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="View" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDITROW" ID="cmdView" ToolTip="Edit/View"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Approve Reschedule" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                    CommandName="APPROVERESCHEDULE" ID="imgApprove" ToolTip="Approve"></asp:ImageButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="3" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>

</body>
</html>
