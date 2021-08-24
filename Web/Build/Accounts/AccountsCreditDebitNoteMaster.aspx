<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditDebitNoteMaster.aspx.cs"
    Inherits="AccountsCreditDebitNoteMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Credit Debit Note Master</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("gvFormDetails");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 130) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body>
    <form id="frmCreditDebitNoteMaster" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuCreditDebitNoteTab" runat="server" OnTabStripCommand="MenuCreditDebitNoteTab_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="height: 100%; width: 100%"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Scrolling="None">
                <eluc:TabStrip ID="MenuCreditDebitNote" runat="server" OnTabStripCommand="MenuCreditDebitNote_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvCreditDebitNote" runat="server" AutoGenerateColumns="False" Height="88%" CellSpacing="0" GridLines="None"
                    Width="100%" OnItemCommand="gvCreditDebitNote_ItemCommand" OnItemDataBound="gvCreditDebitNote_ItemDataBound"
                    EnableHeaderContextMenu="true" GroupingEnabled="false" AllowSorting="true" EnableViewState="false" AllowPaging="true" AllowCustomPaging="true"
                    OnNeedDataSource="gvCreditDebitNote_NeedDataSource" OnRowCreated="gvCreditDebitNote_RowCreated"
                    OnSelectedIndexChanging="gvCreditDebitNote_SelectedIndexChanging">
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDCREDITDEBITNOTEID">
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
                            <telerik:GridTemplateColumn HeaderText="Supplier Code" HeaderStyle-Width="7%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCreditDebitNoteId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEBITNOTEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCurrencyCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSupplierCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbltabflag" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTABFLAG") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier Name" HeaderStyle-Width="17%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vendor Credit Note No" HeaderStyle-Width="15%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="lnkRefNumber" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEBITNOTEID") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO")  %>'></asp:LinkButton>--%>
                                    <telerik:RadLabel ID="lblRefNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="5%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="5%">
                                <HeaderStyle  HorizontalAlign="Right"/>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Credit Note Register No" HeaderStyle-Width="10%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCNRegisterNo" runat="server" CommandName="CREDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCNREGISTERNO") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCNREGISTERNO")  %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblCNRegisterNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCNREGISTERNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="9%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Funds Status" HeaderStyle-Width="8%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFundsStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDSTATUSNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Auto Adjust" HeaderStyle-Width="6%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAdjustInvoice" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADJUSTINVOICE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadCheckBox ID="chkAdjustInvoice" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDADJUSTINVOICE").ToString().Equals("1"))?true:false %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEBITNOTEID") %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItem %>' ID="cmdAtt"
                                        ToolTip="Attachment"></asp:ImageButton>
                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Create Fund Request" ImageUrl="<%$ PhoenixTheme:images/move-confirm.png %>"
                                        CommandName="CREATEFUNDREQUEST" CommandArgument='<%# Container.DataItem %>' ID="cmdfndrq"
                                        ToolTip="Create Fund Request"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItem %>"
                                        CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
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
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
