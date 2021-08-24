<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDocumentsDetailsFrame.aspx.cs" Inherits="CrewOffshore_CrewOffshoreDocumentsDetailsFrame" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
       <%-- <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewDocs.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>--%>
    </telerik:RadCodeBlock>
</head>
<body >
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" Font-Size="Smaller">
                <div>
                    <telerik:RadGrid ID="gvCrewDocs" runat="server" AutoGenerateColumns="false"  Height="400px"
                        AllowSorting="false" GroupingEnabled="true" BorderStyle="None"
                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvCertificateSchedule_NeedDataSource"
                        OnItemDataBound="gvCrewDocs_ItemDataBound" OnItemCommand="gvCrewDocs_ItemCommand">
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                            AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="true" GroupLoadMode="Client"
                            GroupHeaderItemStyle-CssClass="center">
                            <NoRecordsTemplate>
                                <table runat="server" width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <GroupHeaderTemplate>
                                <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE").ToString() %>'></telerik:RadLabel>
                            </GroupHeaderTemplate>
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="FLDTYPE" SortOrder="Ascending" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Document Name" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblsubtype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBTYPE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Document Number" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblplace" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Issue Date" HeaderStyle-Width="75px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentIssue" runat="server" Text='<%# General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Expiry Date" HeaderStyle-Width="75px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentexpire" runat="server" Text='<%# General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px" Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="Attachment" CommandArgument="<%# Container.DataItem %>" ID="cmdAtt" ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
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
                </div>

            </telerik:RadAjaxPanel>
        </div>
    </form>

</body>
</html>
