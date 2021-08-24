<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsDashboardOutStandingFundsRequest.aspx.cs" Inherits="Accounts_AccountsDashboardOutStandingFundsRequest" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <div>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblsubtype" Visible="false" runat="server" Text="Sub-Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubtype" Visible="false" runat="server" DataSource='<%# PhoenixRegistersQuick.ListQuick(1, 154)%>' OnTextChanged="ddlSubtype_TextChanged" AutoPostBack="true"
                            DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" OnDataBound="ddlSubtype_DataBound" Width="200px" Filter="Contains" EmptyMessage="Type to select">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvOutstandingFund" runat="server" AutoGenerateColumns="false"
                AllowSorting="false" GroupingEnabled="false" Height="450px" OnItemDataBound="gvOutstandingFund_ItemDataBound"
                EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1500" OnNeedDataSource="gvOutstandingFund_NeedDataSource" >
                <MasterTableView TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Principal" HeaderStyle-Width="40%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPrincipalid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="30-60 days" HeaderStyle-Width="20%" UniqueName="UniqueGThree">
                            <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk33dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESIXCOUNT") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lbl30daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHREESIXURL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="60-90 days" HeaderStyle-Width="20%" UniqueName="UniqueGSix">
                            <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk90dayscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIXNINECOUNT") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lbl90daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIXNINEURL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=">90 days " HeaderStyle-Width="20%" UniqueName="UniqueGNine">
                            <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkgrt90count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRNINECOUNT") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblgrt90url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTRNINEURL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="20%" Visible="false">
                            <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnktotalcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOUNT") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lbltotalurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTOTALURL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                    <Resizing AllowColumnResize="true" />
                </ClientSettings>
                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
            </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
