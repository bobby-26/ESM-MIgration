﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalRAStatus.aspx.cs" Inherits="Dashboard_DashboardTechnicalRA" %>

<%@ Import Namespace="System.Data" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvForms.ClientID %>"));
                }, 200);
           }
           window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel" LoadingPanelID="RadAjaxLoadingPanel1">
            <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadGrid ID="gvForms" runat="server" AutoGenerateColumns="false"
                AllowSorting="false" GroupingEnabled="false" OnItemDataBound="gvForms_ItemDataBound"
                EnableHeaderContextMenu="true" AllowCustomPaging="true" AllowPaging="true" OnNeedDataSource="gvForms_NeedDataSource">
                <MasterTableView TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Ref.No">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblnumber" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDRANUMBER"]%>' ></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>         
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemTemplate>
                                <asp:Label ID="lbltype" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDTYPE"]%>' ></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prepared Date">
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"].ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>              
                        <telerik:GridTemplateColumn HeaderText="Intended Work Date">
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDINTENDEDWORKDATE"].ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target Date for completion">
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCOMPLETIONDATE"].ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity / Conditions">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRisk" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDACTIVITYCONDITIONS"]%>'></asp:LinkButton>
                                <telerik:RadLabel ID="lbltypeid" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDTYPEID"]%>' ></telerik:RadLabel>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
