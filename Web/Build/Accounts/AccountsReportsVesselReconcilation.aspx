<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportsVesselReconcilation.aspx.cs"
    Inherits="AccountsReportsVesselReconcilation" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Reconcilation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="VesselReport" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


            <eluc:TabStrip ID="MenuVesselReport" runat="server" OnTabStripCommand="MenuVesselReport_TabStripCommand"></eluc:TabStrip>

            <br />
            <div id="divFind" style="position: relative; z-index: 2">
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td style="width: 20%">
                            <telerik:RadLabel ID="lblUserAccess" runat="server" Text="User Access Level"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtUserAccess" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td style="width: 20%">
                            <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtCompany" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlType" runat="server" CssClass="input_mandatory" Filter="Contains" EmptyMessage="Type to select">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="Base Currency" />
                                    <telerik:RadComboBoxItem Value="2" Text="Reportting Currency" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td />
                        <td />
                    </tr>
                    <tr>
                        <td colspan="4">
                            <font color="blue">Note: If User Access Level is Normal he can see only Normal Accounts.
                                    <br />
                                If User Access Level is Restricted he can see both Normal and Restricted Accounts.
                                    <br />
                                If User Access Level is Confidential he can see all Accounts. </font>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <table style="float: left; width: 100%;">
                                <tr>
                                    <td style="white-space: nowrap">
                                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account Code/Account Description"></telerik:RadLabel>
                                        <telerik:RadTextBox runat="server" ID="txtAccountSearch" CssClass="input" MaxLength="10"
                                            Width="150px">
                                        </telerik:RadTextBox>&nbsp;
                                            <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                                                ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click" ToolTip="Search" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAccount" runat="server" Text="Account"></telerik:RadLabel>
                        </td>
                        <td>
                            <div runat="server" id="Div1" class="input" style="overflow: auto; width: 80%; height: 220px">
                                <asp:CheckBoxList runat="server" ID="cblAccount" Height="100%" RepeatColumns="1"
                                    AutoPostBack="True" OnSelectedIndexChanged="AccountSelection" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <%-- <asp:GridView ID="gvSelectedAccount" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowDataBound="gvSelectedAccount_ItemDataBound"
                                OnRowDeleting="gvSelectedAccount_RowDeleting" ShowFooter="false" ShowHeader="true"
                                EnableViewState="false">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvSelectedAccount" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSelectedAccount_NeedDataSource"
                                GroupingEnabled="false" EnableHeaderContextMenu="true"
                                OnDeleteCommand="gvSelectedAccount_DeleteCommand"
                                AutoGenerateColumns="false">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed">
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="NO ACCOUNTS SELECTED" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <HeaderStyle Width="102px" />

                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Account" HeaderStyle-Width="40%">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblSelectedAccounts" runat="server" Text="Selected Account(s)"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblAccountName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                                    ToolTip="Delete"></asp:ImageButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                        <td>
                    </tr>
                </table>
            </div>
        </div>

    </form>
</body>
</html>
