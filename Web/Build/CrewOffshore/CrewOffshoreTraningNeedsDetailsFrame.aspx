<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTraningNeedsDetailsFrame.aspx.cs" Inherits="CrewOffshore_CrewOffshoreTraningNeedsDetailsFrame" %>

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
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewTraining.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" Font-Size="Smaller">
            <div>
                <telerik:RadGrid ID="gvCrewTraining" runat="server" AutoGenerateColumns="false"
                    AllowSorting="false" GroupingEnabled="true" BorderStyle="None"
                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvCrewTraining_NeedDataSource"
                    >
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
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                   
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Overdue" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbloverdue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="15 Days" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbl15days" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDPENDINGCBTCOUNT15") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="30 Days" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbl30days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCBTCOUNT30") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="No due" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblanydays" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCBTCOUNTANY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Completed" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblcompleted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETED") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Override" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblcompleted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                        </Columns>
                    </MasterTableView>
                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                        <Resizing AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </telerik:RadAjaxPanel>
    </div>
    </form>
</body>
</html>
