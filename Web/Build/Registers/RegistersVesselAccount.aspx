<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselAccount.aspx.cs"
    Inherits="RegistersVesselAccount" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Account</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselAccounts" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlVesselAccount" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuVesselAccount" runat="server" OnTabStripCommand="VesselAccount_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblMenuVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="150px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVesselAccountId" runat="server" Text="" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblVesselCode" runat="server" Text="Vessel Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselCode" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblVesselAccount" runat="server" Text="Vessel Account"></asp:Literal>
                    </td>
                    <td style="width: 50%">
                        <span id="spnPickListVesselAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                ReadOnly="true" Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                ReadOnly="true" MaxLength="50" Width="160px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowAccount" runat="server" OnClientClick="return showPickList('spnPickListVesselAccount', 'codehelp1', '', '../Common/CommonPickListVesselAccount.aspx',true); ">
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="hidden" MaxLength="20" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblAllowNewPO" runat="server" Text="Allow New PO"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAccessAllowed" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblOfficeExpense" runat="server" Text="Office Expense"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOfficeExpense" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblReportsCurrency" runat="server" Text="Reports Currency"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlReportsCurrency" runat="server" Width="120px" CssClass="input">
                            <Items>
                                <telerik:DropDownListItem Text="Reporting Currency" Value="0" Selected="True"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Vessel Currency" Value="1"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            <table width="100%">
                <tr>
                    <td width="23%">
                        <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselNameSearch" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersVesselAccount" runat="server" OnTabStripCommand="VesselAccountMenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gdVesselAccount" runat="server" AutoGenerateColumns="False" Font-Size="11px" Height="55%"
                GridLines="None" Width="100%" CellPadding="3" OnItemCommand="gdVesselAccount_ItemCommand" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                OnItemDataBound="gdVesselAccount_ItemDataBound" OnNeedDataSource="gdVesselAccount_NeedDataSource"
                OnSortCommand="gdVesselAccount_SortCommand" ShowFooter="false" ShowHeader="true"
                EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="130px" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="EDIT" OnCommand="VesselNameClick"
                                    CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID").ToString()+"^"+DataBinder.Eval(Container,"DataItem.FLDVESSELCODE").ToString()+"^"+DataBinder.Eval(Container,"DataItem.FLDVESSELHISTORYID").ToString() %> '
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Code" HeaderStyle-Width="95px" AllowSorting="true" SortExpression="FLDVESSELCODE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselCode" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Principal" HeaderStyle-Width="225px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Type" HeaderStyle-Width="130px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAccountCode" runat="server" Text="Account Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="AccountCodeDescription" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDecription" runat="server" Text="Description"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Access Allowed" HeaderStyle-Width="115px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAllowNewPO" runat="server" Text="Allow New PO"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccessAllowed" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACCESSALLOWED").ToString().Equals("1")) ? "Yes" :
                                 ((DataBinder.Eval(Container,"DataItem.FLDACCESSALLOWED").ToString().Equals("0")) ? "No": "")  %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Office Expense" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblOfficeExpense" runat="server" Text="Office Expense"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOfficeExpense" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISOFFICEEXPENSE").ToString().Equals("1")) ? "Yes" :
                                 ((DataBinder.Eval(Container,"DataItem.FLDISOFFICEEXPENSE").ToString().Equals("0")) ? "No": "")  %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" HeaderStyle-Width="75px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete"
                                    CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID").ToString()+"^"+DataBinder.Eval(Container,"DataItem.FLDVESSELCODE").ToString()+"^"+DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNTID") %>'>
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="425px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
