<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersSOAWithSupportingsVoucher.aspx.cs" Inherits="OwnersSOAWithSupportingsVoucher" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Statement of Accounts</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmOwnersAccounts" DecoratedControls="All" />
    <form id="frmOwnersAccounts" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <div style="width: 50%; height: 100%; float: left;">
                <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnersAccount" Height="85%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvOwnersAccount_ItemCommand" OnItemDataBound="gvOwnersAccount_ItemDataBound" EnableViewState="false"
                    ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvOwnersAccount_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Voucher Date">
                                <HeaderStyle Width="15%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Voucher Row">
                                <HeaderStyle Width="20%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVoucherDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDTKEY") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <asp:LinkButton ID="lblVoucherRow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERROW") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERROW") %>' CommandName="Select"></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Particulars">
                                <HeaderStyle Width="50%" />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblParticulars" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Amount(USD)">
                                <HeaderStyle Width="10%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n}") %>'></telerik:RadLabel>
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <div style="width: 50%; height: 100%; float: left;">
                <table>
                    <tr>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblAccountCodeDescriptionHeader" runat="server" Text="Account Code / Description"></telerik:RadLabel>
                            </b>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAccountId" Visible="false" runat="server"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAccountCOdeDescription" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblOwnerBudgetCodeDescription" runat="server" Text="Owner Budget Code / Description"></telerik:RadLabel>
                            </b>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lnkOwnerBudgetCode" runat="server"></telerik:RadLabel>
                            &nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;
                                    <telerik:RadLabel ID="lblBudgetCodeDescription" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblStatementReferenceHeader" runat="server" Text="Statement Reference"></telerik:RadLabel>
                            </b>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblStatementReference" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblBudgetCodeTotal" runat="server" Text="Budget Code Total"></telerik:RadLabel>
                            </b>
                        </td>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblUSD" runat="server" Text="USD"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblTotal"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataList runat="server" ID="dlVoucherAttachmentLink" RepeatDirection="Vertical"
                                EnableViewState="false" OnItemCommand="dlVoucherAttachmentLink_RowCommand">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="VLlnkAttachment" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'>
                                    </asp:LinkButton>
                                    &nbsp; &nbsp;
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataList runat="server" ID="dlAttachmentLink" RepeatDirection="Vertical" EnableViewState="false"
                                OnItemCommand="dlAttachmentLink_RowCommand">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkAttachment" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'>
                                    </asp:LinkButton>
                                    &nbsp; &nbsp;
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 50%; vertical-align: top; min-height: 750px; border: 1px solid #CCC;">
                            <div>
                                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 550px; width: 100%" frameborder="0"></iframe>
                                <asp:HiddenField ID="hdnScroll" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <%--<table width="100%">
                <tr>
                    <td style="width: 50%; vertical-align: top;">
                        <table width="100%">
                            <tr>
                                <td style="width: 50%;" valign="top" align="left"></td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top"></td>
                </tr>
            </table>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
