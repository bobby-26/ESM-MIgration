<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportIncidentView.aspx.cs" Inherits="OwnersMonthlyReportIncidentView" %>

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
                   TelerikGridResize($find("<%= gvincident.ClientID %>"));
                }, 200);
           }
        </script>        
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">           
            <table style="width: 100%" runat="server">
                <tr>
                    <td style="width: 100%;">
                        <eluc:TabStrip ID="MenuIncoident" runat="server" OnTabStripCommand="MenuIncoident_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvincident" runat="server" GridLines="None" OnNeedDataSource="gvincident_NeedDataSource"
                        AutoGenerateColumns="false" OnItemDataBound="gvincident_ItemDataBound"
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
                                <telerik:GridTemplateColumn HeaderText="Ref. No" UniqueName="RefNo" AllowSorting="true" SortExpression="FLDINCIDENTREFNO">
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblInspectionIncidentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblStatusid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSOFINCIDENT" ) %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkIncidentRefNo" runat="server" CommandName="INCEDIT"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTREFNO") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Type">
                                    <HeaderStyle Width="6%" HorizontalAlign="Left" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblClassification" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTCLASSIFICATION" ) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Category">
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME" ) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Subcategory">
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME" ) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Cons">
                                    <HeaderStyle Width="4%" HorizontalAlign="Left" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblConsequenceCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSEQUENCECATEGORY" ) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Title">
                                    <HeaderStyle Width="12%" HorizontalAlign="Left" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINCIDENTTITLE").ToString() %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reported" AllowSorting="true" SortExpression="FLDREPORTEDDATE">
                                    <HeaderStyle Width="7%" HorizontalAlign="Left" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReportedBy" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDREPORTEDDATE")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Incident" AllowSorting="true" SortExpression="FLDINCIDENTDATE">
                                    <HeaderStyle Width="7%" HorizontalAlign="Left" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIncidentDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINCIDENTDATE")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSOFINCIDENTNAME" ) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
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
