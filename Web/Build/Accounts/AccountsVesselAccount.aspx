<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselAccount.aspx.cs"
    Inherits="AccountsVesselAccount" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PageNumber" Src="~/UserControls/UserControlPageNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Account</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselAccounts" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuVesselAccount" runat="server" OnTabStripCommand="VesselAccount_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblMenuVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="150px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVesselAccountId" runat="server" Text=""></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblVesselCode" runat="server" Text=" Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselCode" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td style="width: 50%">
                        <span id="spnPickListVesselAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                ReadOnly="true" Width="10%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                ReadOnly="true" MaxLength="50" Width="36%">
                            </telerik:RadTextBox>
                            <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" /> <%--onclick="return showPickList('spnPickListVesselAccount', 'codehelp1', '', '../Common/CommonPickListVesselAccount.aspx',true); "--%>
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAllowNewPO" runat="server" Text="Allow New PO"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAccessAllowed" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOfficeExpense" runat="server" Text="Office Expense"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOfficeExpense" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <font color="blue">
                            <telerik:RadLabel ID="lblNoteDonotmaptheAccountCodeifthedisplayedPrincipalshouldnotbeviewingtheAccountsReports"
                                runat="server" Text="Note: Do not map the Account Code if the displayed Principal should not be viewing the Accounts Reports." Font-Italic="true" ForeColor="DarkBlue">
                            </telerik:RadLabel>
                        </font>
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            <table width="100%">
                <tr>
                    <td width="23%">
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselNameSearch" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersVesselAccount" runat="server" OnTabStripCommand="VesselAccountMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gdVesselAccount" Height="44.5%" RenderMode="Lightweight" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gdVesselAccount_NeedDataSource" OnItemCommand="gdVesselAccount_ItemCommand"
                OnItemDataBound="gdVesselAccount_ItemDataBound" OnSortCommand="gdVesselAccount_SortCommand" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVESSELID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <HeaderStyle Width="12.5%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAccountId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="EDIT" OnCommand="VesselNameClick"
                                    CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID").ToString()+"^"+DataBinder.Eval(Container,"DataItem.FLDVESSELCODE").ToString()+"^"+DataBinder.Eval(Container,"DataItem.FLDVESSELHISTORYID").ToString() %> ' 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDVESSELCODE">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Principal" AllowSorting="true" SortExpression="FLDOWNERNAME">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account" AllowSorting="true" SortExpression="FLDACCOUNTCODE">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Decription" AllowSorting="true" SortExpression="FLDACCOUNTCODEDESCRIPTION">
                            <HeaderStyle Width="17.5%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Allow New PO" AllowSorting="true" SortExpression="FLDACCESSALLOWED">
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccessAllowed" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACCESSALLOWED").ToString().Equals("1")) ? "Yes" :
                                        ((DataBinder.Eval(Container,"DataItem.FLDACCESSALLOWED").ToString().Equals("0")) ? "No": "")  %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" Width="20px" Height="20px" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdDiscount" runat="server" AlternateText="Discount" CommandName="DISCOUNT" Width="20px" Height="20px"
                                    ToolTip="Supplier Discount">
                                    <span class="icon"><i class="fas fa-bars"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdreturn" runat="server" AlternateText="Return" Width="20px" Height="20px"
                                    CommandName="RETUEN" ToolTip="Supplier Return">
                                    <span class="icon"><i class="fas fa-bars"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" />
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
