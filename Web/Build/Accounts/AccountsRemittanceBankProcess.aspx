<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceBankProcess.aspx.cs"
    Inherits="AccountsRemittanceBankProcess" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittance</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
  <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadAjaxPanel runat="server" ID="pnlRemittance">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--                    <eluc:Title runat="server" ID="frmTitle" Text="Remittance"></eluc:Title>--%>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvRemittence" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvRemittence_RowCommand" OnItemDataBound="gvRemittence_ItemDataBound" GroupingEnabled="false" EnableHeaderContextMenu="true"
                         OnDeleteCommand="gvRemittence_RowDeleting" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvRemittence_NeedDataSource"
                        AllowSorting="true"  EnableViewState="false"
                        OnSortCommand="gvRemittence_Sorting"  >
                          <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames ="FLDREMITTANCEINSTRUCTIONIDLIST" >
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
                            <telerik:GridTemplateColumn HeaderText="Check All">
                                <HeaderTemplate>
                                    <telerik:RadCheckBox ID="chkAllRemittance" runat="server" Text="Check All" AutoPostBack="true"
                                        OnPreRender="CheckAll" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remittance Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblRemittenceNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDREMITTANCENUMBER"
                                        ForeColor="White">Remittance Number&nbsp;</telerik:RadLabel>
                                    <img id="FLDREMITTANCENUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemittenceId" runat="server" Visible="false" Text=''></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCurrencyCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkRemittenceid" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCENUMBERLIST")  %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Supplier Name" SortExpression="FLDSUPPLIERNAME" AllowSorting="true">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              <%--  <HeaderTemplate>
                                    <asp:LinkButton ID="lblSupplierName" runat="server" CommandName="Sort" CommandArgument="FLDSUPPLIERNAME"
                                            ForeColor="White">Supplier Name</asp:LinkButton>
                                        <img id="FLDSUPPLIERNAME" runat="server" visible="false" />
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupplierCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Currency" SortExpression="FLDCURRENCYCODE" AllowSorting="true">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               <%-- <HeaderTemplate>
                                    <asp:LinkButton ID="lblCurrency" runat="server" CommandName="Sort" CommandArgument="FLDCURRENCYCODE"
                                            ForeColor="White">Currency</asp:LinkButton>
                                        <img id="FLDCURRENCYCODE" runat="server" visible="false" />
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remittance Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                  <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemittanceamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Withholding Tax Amount" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Net Amount" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblNetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Beneficiary Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBENEFICIARYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Beneficiary Bank SWIFT Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBeneficiaryBankSWIFTCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSWIFTCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Beneficiary Bank Name" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBeneficiaryBankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Beneficiary  Bank Account Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblBeneficiaryBankAccountNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Intermediary Bank SWIFT Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIntermediaryBankSWIFTCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISWIFTCODE","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Intermediary Bank Name" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIntermediaryBankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIBANKNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Intermediary Bank Account Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblIntermediaryBankAccountNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIIBANNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Payment Mode">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               <ItemTemplate>
                                    <telerik:RadLabel ID="lblPaymentmode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEPAYMENTMODENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Account Code" AllowSorting="true" SortExpression="FLDACCOUNTCODE">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <%--    <HeaderTemplate>
                                    <asp:LinkButton ID="lblAccountCode" runat="server" CommandName="Sort" CommandArgument="FLDACCOUNTCODE"
                                            ForeColor="White">Account Code</asp:LinkButton>
                                        <img id="FLDACCOUNTCODE" runat="server" visible="false" />
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Account Description">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Bank Charge Basis">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              <ItemTemplate>
                                    <telerik:RadLabel ID="lblBankChargeBasis" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEBANKCHARGENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="On Hold" ImageUrl="<%$ PhoenixTheme:images/waiting-approve.png %>"
                                        CommandName="OnHold" CommandArgument='<%# Container.DataItem %>' ID="cmdOnHold"
                                        ToolTip="On Hold"></asp:ImageButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
