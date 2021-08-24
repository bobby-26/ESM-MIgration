<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersOperatingExpences.aspx.cs" Inherits="OwnersOperatingExpences" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/OwnersReport.css" rel="stylesheet" />
        <%: Scripts.Render("~/bundles/js") %>
        <script type="text/javascript">
            //Dummy function to ignore javascript page error
            function OnHeaderMenuShowing(sender, args) {
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .rgExpand,
        .rgCollapse {
            display: none !important;
        }

        .panelfont {
            overflow: auto;
            font-size: 11px;
        }

        .RadGrid .rgHeader, .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }

        .higherZIndex {
            z-index: 2;
        }

        .RadGrid_Windows7 .rgHeader {
            color: black !important;
        }

        .rdTitleWrapper, .rdTitleBar {
            background-color: rgb(194, 220, 252) !important;
            background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248)) !important;
            color: black !important;
        }

        .rgGroupCol {
            padding-left: 0 !important;
            padding-right: 0 !important;
            font-size: 1px !important;
        }

        .white {
            color: black !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
            text-align-last: right !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="false">
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <telerik:RadDockZone ID="RadDockZone1" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Height="585px"
                    EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                    EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                    <TitlebarTemplate>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <table width="20%">
                                    <tr>
                                        <td>
                                            <asp:LinkButton
                                                ID="lnkExpenses"
                                                runat="server"
                                                Style="text-decoration: underline; color: black;"
                                                Text="Vessel Operating Expenses">
                                            </asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkExpensesComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>                               
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </TitlebarTemplate>
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table runat="server" id="table1" style="width: 100%; background-color: rgb(194, 220, 252); background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248));">
                            <tr>
                                <td style="width: 50%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 60%">
                                                <telerik:RadLabel ID="lblParticulars" runat="server" Text="Particulars" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 40%">
                                                <telerik:RadLabel ID="lblMonthly" runat="server" Text="Monthly" CssClass="white"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 50%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblCurrentYear" runat="server" Text="Current Year" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblVarAbs" runat="server" Text="Var(Abs)" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblVarPer" runat="server" Text="Var(%)" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblDailyAvgs" runat="server" Text="Daily Avg" CssClass="white"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 50%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 60%">
                                                <telerik:RadLabel ID="lblNoofdays" runat="server" Text="No of Days" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 40%">
                                                <telerik:RadLabel ID="lblmonth" runat="server" Text="" CssClass="white"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 50%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblyeardays" runat="server" Text="330" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblbudgetdays" runat="server" Text="330" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblvardays" runat="server" Text="330" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblVarperdays" runat="server" Text="" CssClass="white"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 20%; text-align: center;">
                                                <telerik:RadLabel ID="lblVaravgdays" runat="server" Text="YEARLY" CssClass="white"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%" runat="server">
                            <tr>
                                <td style="width: 50%; vertical-align: top;">

                                    <telerik:RadGrid ID="GVPUR" runat="server" AutoGenerateColumns="false" OnItemCreated="gvYearly_ItemCreated"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="GVPUR_NeedDataSource"
                                        OnItemDataBound="GVPUR_ItemDataBound" ShowFooter="true" OnColumnCreated="GVPUR_ColumnCreated" OnItemCommand="GVPUR_ItemCommand">
                                        <MasterTableView TableLayout="Fixed" ShowGroupFooter="true">
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldName="FLDBUDGETTYPE" SortOrder="Ascending" />
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="FLDBUDGETTYPE" SortOrder="Ascending" />
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Expenses" HeaderStyle-Wrap="false" HeaderStyle-Width="40%" UniqueName="Expenses">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <%--<FooterTemplate>
                                                        <telerik:RadLabel runat="server" ID="lblfooter" Text="Total :"></telerik:RadLabel>
                                                    </FooterTemplate>--%>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Actuals" HeaderStyle-Wrap="false" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                                    Aggregate="Sum" FooterText=" " DataField="FLDTOTAL">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblAutuals" CommandName="ACTUAL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Budgeted" HeaderStyle-Wrap="false" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                                    Aggregate="Sum" FooterText=" " DataField="FLDBUDGET">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblBudgeted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGET") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Var(Abs)" HeaderStyle-Wrap="false" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                                    Aggregate="Sum" FooterText=" " DataField="FLDVARIANCE">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblVar" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Var(%)" HeaderStyle-Wrap="false" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblVarper" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCEPERCENTAGE") %>'></telerik:RadLabel>
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
                                <td style="width: 50%; vertical-align: top;">
                                    <telerik:RadGrid ID="gvYearly" runat="server" AutoGenerateColumns="false" OnItemDataBound="gvYearly_ItemDataBound"
                                        AllowSorting="false" GroupingEnabled="True" BorderStyle="None" OnItemCreated="gvYearly_ItemCreated" OnColumnCreated="gvYearly_ColumnCreated"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvYearly_NeedDataSource" ShowFooter="true"
                                        OnItemCommand="gvYearly_ItemCommand">
                                        <MasterTableView TableLayout="Fixed" ShowGroupFooter="true">
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldName="FLDBUDGETTYPE" SortOrder="Ascending" />
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="FLDBUDGETTYPE" SortOrder="Ascending" />
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn Visible="false" HeaderText="Expences" HeaderStyle-Wrap="false" HeaderStyle-Width="50%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <telerik:RadLabel ID="lblfootertext" runat="server" Text="Grand Total"></telerik:RadLabel>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Current Year" FooterStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                                    Aggregate="Sum" FooterText=" " DataField="FLDTOTAL">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblAutuals" CommandName="ACTUAL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Current Year" FooterStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                                    Aggregate="Sum" FooterText=" " DataField="FLDBUDGET">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblBudgeted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGET") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Current Year" FooterStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                                    Aggregate="Sum" FooterText=" " DataField="FLDVARIANCE">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblVar" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Current Year" HeaderStyle-Wrap="false" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblVarper" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCEPERCENTAGE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Actuals" HeaderStyle-Wrap="false" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDailyAvg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAILYAVERAGE") %>'></telerik:RadLabel>
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
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <%--            </telerik:RadDockLayout>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
