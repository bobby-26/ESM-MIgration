<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceBatchList.aspx.cs"
    Inherits="AccountsRemittanceBatchList" %>

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
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvRemittence.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
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
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--                    <eluc:Title runat="server" ID="frmTitle" Text="Remittance"></eluc:Title>--%>
        <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="Both">
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 98%; width: 99.7%"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Scrolling="None">
                <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvRemittence" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnItemCommand="gvRemittence_RowCommand" OnItemDataBound="gvRemittence_ItemDataBound"
                    OnDeleteCommand="gvRemittence_RowDeleting" AllowPaging="true" AllowCustomPaging="true" GroupingEnabled="false" EnableHeaderContextMenu="true" OnNeedDataSource="gvRemittence_NeedDataSource"
                    AllowSorting="true" EnableViewState="false"
                    OnSortCommand="gvRemittence_Sorting">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDBATCHID">
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
                            <telerik:GridTemplateColumn HeaderText="Voucher Number" AllowSorting="true" SortExpression="FLDBATCHNUMBER">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <%--   <HeaderTemplate>
                                    <telerik:RadLabel ID="lblRemittenceNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDBATCHNUMBER"
                                        ForeColor="White">Batch Number&nbsp;</telerik:RadLabel>
                                    <img id="FLDBATCHNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBatchId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkBatchId" runat="server" CommandName="EDITBATCH" CommandArgument='<%# Container.DataItem%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNUMBER")  %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Payment date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPaymentDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:UserControlDate ID="txtPaymentDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTDATE","{0:dd/MMM/yyyy}") %>'
                                        CssClass="input_mandatory" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Payment Mode">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPaymentmode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Account Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNT") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNTID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--   <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Bank Instruction 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFileName"  runat="server" Visible = "false"  Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'> </telerik:RadLabel>
                                    <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                        Height="14px" ToolTip="Download File">
                                        <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH") %>'></telerik:RadLabel>
                                    </asp:HyperLink></ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Post" ImageUrl="<%$ PhoenixTheme:images/completed.png %>"
                                        CommandName="APPROVE" CommandArgument='<%# Container.DataItem %>' ID="cmdPost"
                                        ToolTip="Post"></asp:ImageButton>
                                    <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <telerik:RadLabel ID="lblFileName" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVoucherid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblIsmodified" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDISBATCHMODIFIED") %>'></telerik:RadLabel>
                                    <asp:ImageButton ID="cmdHistory" runat="server" AlternateText="History" CommandArgument='<%# Container.DataItem %>'
                                        CommandName="History" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png%>" ToolTip="History" />
                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="imgUpload" runat="server" AlternateText="Bank Upload" CommandArgument='<%# Container.DataItem %>'
                                        CommandName="BANKUPLOAD" ImageUrl="<%$ PhoenixTheme:images/upload.png%>" Visible="false"
                                        ToolTip="Generate Bank Upload" />
                                    <img id="Img8" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:HyperLink ID="lnkfilename" Target="_blank" Text="Bank download" runat="server"
                                        Width="90" Visible="false" Height="14px" ToolTip="Download File">
                                        <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH") %>'></telerik:RadLabel>
                                    </asp:HyperLink>
                                    <img id="Img7" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%--                                    <telerik:RadLabel ID="lblIsmodified" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDISBATCHMODIFIED") %>'></telerik:RadLabel>
                                    --%>
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
