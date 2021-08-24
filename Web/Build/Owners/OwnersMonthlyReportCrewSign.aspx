<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportCrewSign.aspx.cs" Inherits="OwnersMonthlyReportCrewSign" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvSignoff.ClientID %>"));
                }, 200);
           }
        </script>  
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <telerik:RadGrid ID="gvSignoff" runat="server" ShowHeader="true" ShowFooter="false" AutoGenerateColumns="false"
                GridLines="None" BorderStyle="None" 
                OnNeedDataSource="gvSignoff_NeedDataSource" OnItemDataBound="gvSignoff_ItemDataBound">
                <MasterTableView ShowHeader="true" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Off-Signer">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignoffName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignoffRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignoffNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNATIONALITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Wages">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignoffWages" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERWAGES") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign off Date">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignoffdate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERSIGNOFFDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignoffPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason for Sign Off">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Wrap="true" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignoffReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="On-Signer">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERNATIONALITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Wages">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWages" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERWAGES") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign on Date">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignondate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDONSIGNERSIGNONDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Documents">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocument" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERDOCUMENTS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>