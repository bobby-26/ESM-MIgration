<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectList.aspx.cs"
    Inherits="AccountsProjectList" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Direct PO</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .rwContent {
                color: red !important;
            }

            .rwTitle {
                font-weight: 600 !important;
            }
        </style>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("gvDPO");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 85) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <form id="frmInvoiceDirctPO" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlVoucher" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <eluc:TabStrip ID="MenuDPO" runat="server" OnTabStripCommand="MenuDPO_TabStripCommand"
                TabStrip="true">
            </eluc:TabStrip>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="height: 100%; width: 99%"></iframe>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                    <asp:HiddenField ID="hdnScroll" runat="server" />
                    <eluc:TabStrip ID="MenuProject" runat="server" OnTabStripCommand="MenuProject_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvDPO" runat="server" AutoGenerateColumns="False" Font-Size="11px" EnableHeaderContextMenu="true" GroupingEnabled="false"
                        Height="90%" Width="100%" CellPadding="3" OnItemCommand="gvDPO_ItemCommand" ShowHeader="true" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvDPO_NeedDataSource"
                        EnableViewState="false" AllowSorting="true" OnSelectedIndexChanging="gvDPO_SelectedIndexChanging"
                        OnItemDataBound="gvDPO_ItemDataBound" OnRowCommand="gvDPO_RowCommand">
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDID">
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
                                <telerik:GridTemplateColumn HeaderText="Project Code" HeaderStyle-Width="20%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%" ></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblprojectId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblaccountid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>

                                        <asp:LinkButton ID="lnkFormNo" runat="server" CommandName="select" CommandArgument='<%# Container.DataItem %>'><%# ((DataRowView)Container.DataItem)["FLDPROJECTCODE"]%></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Project Type" HeaderStyle-Width="25%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDTYPENAME"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="10%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" Width="20%" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                            CommandName="SELECT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
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
                    <%-- <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">--%>
                    <%--                <tr>
                    <td nowrap align="center">
                        <telerik:RadLabel ID="lblPagenumber" runat="server">
                        </telerik:RadLabel>
                        <telerik:RadLabel ID="lblPages" runat="server">
                        </telerik:RadLabel>
                        <telerik:RadLabel ID="lblRecords" runat="server">
                        </telerik:RadLabel>&nbsp;&nbsp;
                    </td>
                    <td nowrap align="left" width="50px">
                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                    </td>
                    <td width="20px">&nbsp;
                    </td>
                    <td nowrap align="right" width="50px">
                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                    </td>
                    <td nowrap align="center">
                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                        </asp:TextBox>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
            </table>--%>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
