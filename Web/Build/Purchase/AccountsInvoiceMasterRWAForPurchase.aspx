<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceMasterRWAForPurchase.aspx.cs"
    Inherits="AccountsInvoiceMasterRWAForPurchase" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            .datagrid_selectedstyle1 {
                color: Black;
                height: 10px;
            }

            .odd {
                background-color: white;
            }

            .even {
                background-color: gray;
            }
        </style>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
<body onload="resizeDiv()" onresize="resizeDiv()">
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
       <%-- <telerik:RadAjaxPanel ID="pnlInvoice" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--<eluc:Title runat="server" ID="frmTitle" Text="Invoice Register"></eluc:Title> --%>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"/>
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="height: 100%; width: 100%"></iframe>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="RadPane1" runat="server" Scrolling="None">
                    <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvInvoice" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvInvoice_ItemCommand" OnItemDataBound="gvInvoice_ItemDataBound"
                        OnRowDeleting="gvFormDetails_RowDeleting" AllowPaging="true"  AllowCustomPaging="true" Height="90%" 
                        AllowSorting="true" EnableViewState="false" OnNeedDataSource="gvInvoice_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
                        OnSortCommand="gvInvoice_Sorting" OnSelectedCellChanging="gvInvoice_SelectedIndexChanging">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDINVOICECODE">
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
                                <telerik:GridTemplateColumn HeaderText="PO Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEORDERNUMBER") %>'></telerik:RadLabel>
                                   <telerik:RadTextBox ID="txtOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                    MaxLength="10" Visible="false">
                                </telerik:RadTextBox>

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Invoice Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblInvoiceid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkInvoice" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex%>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER")  %>'></asp:LinkButton>
                                        <telerik:RadTextBox ID="txtInvoiceCode" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDINVOICECODE") %>'
                                            Visible="false">
                                        </telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Supplier Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblBackToBack" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Supplier Invoice Reference">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLeave" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESUPPLIERREFERENCE") %>'></telerik:RadLabel>
                                        <%--<telerik:RadLabel ID="lblAttachmentexists" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENTEXISTS") %>'
                                    Visible="false"></asp:Label>--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Received Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICERECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText=" Vessel Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText=" Currency Code">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Invoice Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblInvoiceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Invoice Type">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblInvoiceType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICETYPENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Invoice Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUSNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="PO Amount (USD)" AllowSorting="true" SortExpression="FLDPOUSDAMOUNT">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPOamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOUSDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                               
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="cmdPOApprove" runat="server" AlternateText="POApprove" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="POAPPROVE" ImageUrl="<%$ PhoenixTheme:images/approve.png %>" ToolTip="PO Approve"
                                         Visible="true" />
                                
                                       <%-- <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" Visible="false" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton ID="imgAttachment" Visible="false" runat="server" AlternateText="Attachment" CommandArgument="<%# Container.DataSetIndex %>"
                                            CommandName="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png%>"
                                            ToolTip="Attachment" />--%>
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
                </telerik:RadPane>
            </telerik:RadSplitter>
        <%--</telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
