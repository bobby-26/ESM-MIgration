<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerOperatingExpensesView.aspx.cs" Inherits="OwnerOperatingExpensesView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .white {
            color: white !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
            text-align-last: right !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <table runat="server" id="table1" style="width: 100%; background-color: rgb(28, 132, 198);">
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
                                        Aggregate="Sum" FooterText=" " DataField="FLDTOTAL" UniqueName="Actuals">
                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblAutuals" CommandName="ACTUAL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Budgeted" HeaderStyle-Wrap="false" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                        Aggregate="Sum" FooterText=" " DataField="FLDBUDGET" UniqueName="Budgeted">
                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBudgeted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGET") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Var(Abs)" HeaderStyle-Wrap="false" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                        Aggregate="Sum" FooterText=" " DataField="FLDVARIANCE" UniqueName="Var(Abs)">
                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVar" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Var(%)" HeaderStyle-Wrap="false" HeaderStyle-Width="15%" 
                                        HeaderStyle-HorizontalAlign="Center" UniqueName="Varpercent">
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
                                        Aggregate="Sum" FooterText=" " DataField="FLDTOTAL" UniqueName="FLDTOTAL">
                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblAutuals" CommandName="ACTUAL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Current Year" FooterStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                        Aggregate="Sum" FooterText=" " DataField="FLDBUDGET" UniqueName="FLDBUDGET">
                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBudgeted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGET") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Current Year" FooterStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                        Aggregate="Sum" FooterText=" " DataField="FLDVARIANCE" UniqueName="FLDVARIANCE">
                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVar" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Current Year" HeaderStyle-Wrap="false" HeaderStyle-Width="20%" 
                                        HeaderStyle-HorizontalAlign="Center" UniqueName="FLDVARIANCEPERCENTAGE">
                                        <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVarper" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCEPERCENTAGE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Actuals" HeaderStyle-Wrap="false" HeaderStyle-Width="20%"
                                         HeaderStyle-HorizontalAlign="Center" UniqueName="FLDDAILYAVERAGE">
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

