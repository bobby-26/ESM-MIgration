<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersContractCBAExpression.aspx.cs"
    Inherits="RegistersContractCBAExpression" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CBA Component Expression</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>

                        <telerik:RadTextBox ID="txtComponent" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblFormula" runat="server" Text="Formula"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtExpression" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" TextMode="MultiLine" Rows="2" Columns="150" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblDoubleClick" runat="server" Text="Note : Double Click to build formula"
                            ForeColor="Blue"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExprComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOperator" runat="server" Text="Operator"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="lstComponent" runat="server" Width="250px"></telerik:RadListBox>
                    </td>
                    <td>
                        <telerik:RadListBox ID="lstOperator" runat="server" Width="150px">
                            <Items>
                                <telerik:RadListBoxItem Text="(" Value="("></telerik:RadListBoxItem>
                                <telerik:RadListBoxItem Text=")" Value=")"></telerik:RadListBoxItem>
                                <telerik:RadListBoxItem Text="+" Value="+"></telerik:RadListBoxItem>
                                <telerik:RadListBoxItem Text="-" Value="-"></telerik:RadListBoxItem>
                                <telerik:RadListBoxItem Text="*" Value="*"></telerik:RadListBoxItem>
                                <telerik:RadListBoxItem Text="%" Value="%"></telerik:RadListBoxItem>
                            </Items>
                        </telerik:RadListBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFormula" Width="50%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" OnItemCommand="gvFormula_ItemCommand" OnItemDataBound="gvFormula_ItemDataBound" EnableViewState="false"
                ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvFormula_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">
                    <HeaderStyle Width="102px" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Formula">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFORMULA")%>
                                <asp:Label ID="lblDtKey" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
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
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
