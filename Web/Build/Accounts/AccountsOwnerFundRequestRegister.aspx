<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOwnerFundRequestRegister.aspx.cs"
    Inherits="AccountsOwnerFundRequestRegister" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Owner Fund Request Template </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOpeningsummary" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />


            <eluc:TabStrip ID="MenuOfficeFund" runat="server" OnTabStripCommand="MenuOfficeFund_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <table cellpadding="2" cellspacing="2" width="50%">
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtownerfundrequesttemplateid" runat="server" Visible="false"></telerik:RadTextBox>
                        <telerik:RadLabel ID="lblAccount" runat="server" Text="Account "></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="20" Width="70px">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="hidden" Enabled="false"
                                MaxLength="50" Width="0px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="hidden" MaxLength="20" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblbilltoCompany" runat="server" Text="Bill To Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" OnTextChangedEvent="ddlCompany_selectedtextchange"
                            AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBankreceivingfunds" runat="server" Text="Default Bank Receiving Funds "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlBankreceivingfunds" AutoPostBack="true" Width="240px" EnableLoadOnDemand="true">
                        </telerik:RadComboBox>
                        <%-- <asp:DropDownList ID="ddlBankreceivingfunds" runat="server" CssClass="dropdown_mandatory"
                                    Width="240px">
                                </asp:DropDownList>--%>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lbldefaultcurrency" runat="server" Text="Default Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            ID="ucCurrency" AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcheckBox" runat="server" Text="Default FundRequest Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAddress" TextMode="MultiLine" Rows="4" Width="270px" Height="75px" Resize="Both" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>

            </table>


            <eluc:TabStrip ID="MenuGrid" runat="server" OnTabStripCommand="MenuGrid_TabStripCommand"></eluc:TabStrip>


            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvOwnerFund" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnerFund" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvOwnerFund_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Height="71%" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvOwnerFund_SelectedIndexChanging"
                OnItemDataBound="gvOwnerFund_ItemDataBound" OnItemCommand="gvOwnerFund_ItemCommand" OnEditCommand="gvOwnerFund_OnEditing"
                ShowFooter="false" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDOWNERFUNDREQUESTTEMPLATEID">

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Account">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblownerfundrequesttemplateid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERFUNDREQUESTTEMPLATEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bill To Company">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompanyid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLTOCOMPANYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Default Bank Receiving Funds">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDefaultBankreceivingfundsid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsActiveBankaccount" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISACTIVEBANKACCOUNT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDefaultBankreceivingfunds" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKRECEIVINGFUNDSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblDefaultReference" runat="server"> Default Reference No  </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselshortcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELSHORTCODE") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDefaultReferenceno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Default Currency">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrencyid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Default Address">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChkAddress" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERFUNDADDRESSCHECKEDYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDefAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERFUNDADDRESS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Delete" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

