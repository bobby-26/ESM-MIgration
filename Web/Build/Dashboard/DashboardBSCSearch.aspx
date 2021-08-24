<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCSearch.aspx.cs" Inherits="Dashboard_DashboardBSCSearch" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Score Card</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvPI" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="TabstripMenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table style="margin-left: 10px; width: 95%">
                    <tr>
                        <td>
                            <telerik:RadLabel runat="server" Text="Level" />
                        </td>
                        <td>&nbsp &nbsp
                        </td>
                        <td>
                            <telerik:RadRadioButtonList runat="server" ID="RadRadioButtonkpilevel" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RadRadioButtonkpilevel_SelectedIndexChanged">

                                <Items>
                                    <telerik:ButtonListItem runat="server" Text="Corporate" Value="Corporate" Selected="true" />

                                    <telerik:ButtonListItem runat="server" Text="Department" Value="Department" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                        <td>&nbsp &nbsp
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" Text="Department" />
                        </td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="radcbdept" Width="120px"
                                EmptyMessage="Type to select Department" AllowCustomText="true" OnTextChanged="radcbdept_TextChanged"
                                AutoPostBack="true" />
                        </td>
                        <td>&nbsp &nbsp
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" Text="KPI" />
                        </td>
                        <td>&nbsp &nbsp
                        </td>
                        <td>
                            <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="RadComboKpilist" runat="server" Width="290px" Height="150px" NoWrap="true"
                                DataTextField="FLDKPINAME" DataValueField="FLDKPIID" DropDownWidth="300px" AutoPostBack="true"
                                Placeholder="Type to Select the KPI" Filter="Contains" FilterFields="FLDKPICODE, FLDKPINAME">
                                <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                                </NoDataTemplate>
                                <ColumnsCollection>
                                    <telerik:MultiColumnComboBoxColumn Field="FLDKPICODE" Title="Code" Width="70px" />
                                    <telerik:MultiColumnComboBoxColumn Field="FLDKPINAME" Title="Name" Width="200px" />
                                </ColumnsCollection>
                            </telerik:RadMultiColumnComboBox>
                        </td>
                        <td>&nbsp &nbsp
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" Text="Month" />
                        </td>
                        <td>&nbsp &nbsp
                        </td>
                        <td>
                            <eluc:Month runat="server" ID="radcbmonth" Width="100px" />
                        </td>
                        <td>&nbsp &nbsp
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" Text="Year" />
                        </td>
                        <td>&nbsp &nbsp
                        </td>
                        <td>
                            <eluc:Year runat="server" YearStartFrom="2018" NoofYearFromCurrent="0" ID="radcbyear" Width="70px" />
                        </td>
                    </tr>
                </table>
                <eluc:TabStrip ID="TabScorecard" runat="server" OnTabStripCommand="TabScorecard_TabStripCommand" TabStrip="true"></eluc:TabStrip>
             <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvKPISC" AutoGenerateColumns="false"
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvKPISC_NeedDataSource" Height="80%"
                OnItemDataBound="gvKPISC_ItemDataBound" OnItemCommand="gvKPISC_ItemCommand" ShowFooter="false">
                <MasterTableView EditMode="InPlace" DataKeyNames="FLDKPISCID" AutoGenerateColumns="false"
                    TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>


                                <asp:LinkButton ID="nameanchor" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDKPINAME")%>'>

                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Month">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true"  />
                            <ItemStyle HorizontalAlign="Center" Wrap="true" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblmonth" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMONTHNAME")%>'>
                                </telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Year">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true"  />
                            <ItemStyle HorizontalAlign="Center" Wrap="true" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblyear" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEAR")%>'>
                                </telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75px" Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
