<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportUnsafeActsView.aspx.cs" Inherits="OwnersMonthlyReportUnsafeActsView" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvunsafeact.ClientID %>"));
                }, 200);
           }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%">
            <table style="width: 100%" runat="server">
                <tr>
                    <td style="width: 100%;">
                        <eluc:TabStrip ID="MenuUnSafeact" runat="server" OnTabStripCommand="MenuUnSafeact_TabStripCommand"></eluc:TabStrip>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvunsafeact" runat="server" GridLines="None" OnNeedDataSource="gvunsafeact_NeedDataSource"
                             AutoGenerateColumns="false" OnItemDataBound="gvunsafeact_ItemDataBound"
                            GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false">
                            <MasterTableView TableLayout="Fixed">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Ref. No">
                                        <HeaderStyle Width="11%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRefno" runat="server" CommandName="UNSEDIT"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTNEARMISSREFNO") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Reported">
                                        <HeaderStyle Width="7%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblReportedDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREPORTEDDATE")) %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblDirectIncidentId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDIRECTINCIDENTID")) %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Incident">
                                        <HeaderStyle Width="7%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINCIDENTDATE")) %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Time">
                                        <HeaderStyle Width="5%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTTIME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Category">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDICCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Sub-category">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDICSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Details">
                                        <HeaderStyle Width="12%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSummaryFirstLine" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUMMARY").ToString().Length>35 ? DataBinder.Eval(Container, "DataItem.FLDSUMMARY").ToString().Substring(0, 35)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSUMMARY").ToString() %>'></telerik:RadLabel>
                                            <eluc:ToolTip ID="ucToolTipSummary" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUMMARY") %>' TargetControlId="lblSummaryFirstLine" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Location">
                                        <HeaderStyle Width="9%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATION") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action Taken">
                                        <HeaderStyle Width="14%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActionTaken" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVEACTION").ToString().Length>35 ? DataBinder.Eval(Container, "DataItem.FLDCORRECTIVEACTION").ToString().Substring(0, 35)+ "..." : DataBinder.Eval(Container, "DataItem.FLDCORRECTIVEACTION").ToString() %>'></telerik:RadLabel>
                                            <eluc:ToolTip ID="ucToolTipActionTaken" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVEACTION") %>' TargetControlId="lblActionTaken" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Status">
                                        <HeaderStyle Width="6%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                <Resizing AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
    </form>
</body>
</html>
