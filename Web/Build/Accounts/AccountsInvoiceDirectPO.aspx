<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceDirectPO.aspx.cs"
    Inherits="AccountsInvoiceDirectPO" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
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
    <form id="frmInvoiceDirctPO" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuDirectPO" runat="server" OnTabStripCommand="MenuDirectPO_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuDPO" runat="server" OnTabStripCommand="MenuDPO_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" style="height: 100%; width: 100%"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="RadPane1" runat="server" Scrolling="None">
                <eluc:TabStrip ID="MenuAddAmosPO" runat="server" OnTabStripCommand="MenuAddAmosPO_TabStripCommand"></eluc:TabStrip>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvDPO" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnItemCommand="gvDPO_ItemCommand" OnItemDataBound="gvDPO_ItemDataBound" OnNeedDataSource="gvDPO_NeedDataSource"
                    ShowFooter="false" ShowHeader="true" EnableViewState="false" OnSortCommand="gvDPO_SortCommand" OnSelectedIndexChanging="gvDPO_SelectedIndexChanging" EnableHeaderContextMenu="true" GroupingEnabled="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDORDERID">
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
                            <telerik:GridTemplateColumn HeaderText="PO Number" AllowSorting="true" SortExpression="FLDFORMNO">
                                <HeaderStyle Width="20%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkFormNo" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex%>'><%# ((DataRowView)Container.DataItem)["FLDFORMNO"]%></asp:LinkButton>
                                    <telerik:RadLabel ID="lblPOType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vendor" AllowSorting="true" SortExpression="FLDNAME">
                                <HeaderStyle Width="35%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDNAME"]%>
                                    <telerik:RadLabel ID="lblVendorId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" SortExpression="FLDVESSELNAME">
                                <HeaderStyle Width="25%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"]%>
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency" AllowSorting="true" SortExpression="FLDCURRENCYNAME">
                                <HeaderStyle Width="25%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDCURRENCYNAME"]%>
                                    <telerik:RadLabel ID="lblCurrencyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblIsPoTaggedToInvoice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPOTAGGEDTOINVOICE") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PO Received Date" AllowSorting="true" SortExpression="FLDRECEIVEDDATE">
                                <HeaderStyle Width="25%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDRECEIVEDDATE"])%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="View" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <asp:ImageButton ID="cmdPOApprove" runat="server" AlternateText="POApprove" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="POAPPROVE" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                        ToolTip="PO Approve" Enabled="true" Visible="false" />
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
    </form>
</body>
</html>
