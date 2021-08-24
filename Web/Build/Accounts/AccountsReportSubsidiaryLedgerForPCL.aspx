<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportSubsidiaryLedgerForPCL.aspx.cs"
    Inherits="AccountsReportSubsidiaryLedgerForPCL" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AccountsReportAnnualLedgerAccount</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="AccReport" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="70%">
            <%--<div>--%>
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
                <eluc:TabStrip ID="AnnualReport" runat="server" OnTabStripCommand="AnnualReport_TabStripCommand"></eluc:TabStrip>
              <%--  <div id="divFind" style="position: relative; z-index: 2">--%>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblUserAccess" runat="server" Text="User Access Level"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtUserAccess" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                            </td>
                            <td>
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
                                <telerik:RadDropDownList ID="ddlType" runat="server" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:DropDownListItem Value="1" Text="Base Currency"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="2" Text="Reporting Currency"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="3" Text="Prime Currency"></telerik:DropDownListItem>
                                    </Items>
                                </telerik:RadDropDownList>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcurrency" runat="server" Text="Currency"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Currency runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="140px"
                                    ID="ucCurrency" AppendDataBoundItems="true" AutoPostBack="true" />

                            </td>
                        </tr>
                        <td colspan="4">
                            <font color="blue">Note: If User Access Level is Normal he can see only Normal Accounts.
                        <br />
                        If User Access Level is Restricted he can see both Normal and Restricted Accounts.
                        <br />
                        If User Access Level is Confidential he can see all Accounts.
                        </font>
                        </td>
                        <tr>
                            <td></td>
                            <td>
                                <table style="float: left; width: 100%;">
                                    <tr>
                                        <td style="white-space: nowrap">
                                            <telerik:RadLabel ID="lblAccountCodeAccountDescription" runat="server" Text="Account Code/Account Description"></telerik:RadLabel>
                                            <telerik:RadTextBox runat="server" ID="txtAccountSearch" CssClass="input" MaxLength="10"
                                                Width="150px">
                                            </telerik:RadTextBox>
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
                                    <telerik:RadRadioButtonList ID="rblAccount" runat="server" Height="90%" Columns="1" DataBindings-DataTextField="FLDaccoandept"
                                        Direction="Horizontal" Layout="Flow" AutoPostBack="True" OnSelectedIndexChanged="AccountSelection" DataBindings-DataValueField="FLDACCOUNTID">
                                    </telerik:RadRadioButtonList>
                                </div>
                            </td>
                           <td>
                                <telerik:RadButton runat="server" ID="btnselectedsubaccount" Text="Shows Selected Sub Account" OnClick="btnselectedsubaccount_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSubAccount" runat="server" Text="Sub Account"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <div runat="server" id="dvaccount" class="input" style="overflow: auto; width: 80%; height: 220px">
                                   <asp:CheckBoxList ID="cblSubAccount" runat="server" AutoPostBack="True" Height="100%" OnSelectedIndexChanged="SubAccountSelection" RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                </div>
                            </td>
                             
                        </tr>
                    </table>
               <%-- </div>
            </div>--%>
            <table style="height: 23%">
                <tr>
                    <td style="width: 16%"></td>
                    <td style="height: 100%">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvSelectedAccount" Height="50%" runat="server" AllowSorting="true" Width="50%"
                            CellSpacing="0" GridLines="None" OnItemCommand="gvSelectedAccount_ItemCommand" OnNeedDataSource="gvSelectedAccount_NeedDataSource"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
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
                                    <telerik:GridTemplateColumn HeaderText="Selected Sub Account(s)">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSubAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTMAPID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblSubAccountName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="DELETE" ID="cmdDelete" ToolTip="Delete"></asp:ImageButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
