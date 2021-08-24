<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCaptainCashDraft.aspx.cs"
    Inherits="AccountsCaptainCashDraft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Voucher Draft View</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
            body, html {
                width: 100%;
                height: 100%;
                margin: 0px;
                padding: 0px;
                background-color: #FFFFFF;
            }

            .hidden {
                display: none;
            }

            .center {
                background: fixed !important;
            }

            .modal {
                border: 0px;
                padding: 0px;
                margin: 0px;
                top: 0px;
                left: 0px;
                width: 100%;
                position: absolute;
                background-color: #000000;
                opacity: 0.5;
                filter: Alpha(opacity:50);
                z-index: 11;
                -moz-opacity: 0.8;
                min-height: 100%;
            }

            .loading {
                border: 0px;
                padding: 0px;
                display: none;
                position: absolute;
                border: 5px solid darkgrey;
                background-color: #FFFFFF;
                z-index: 12;
            }
        </style>
        <script type="text/javascript">
            function OffWindowH() {
                var OffWindowH = 0;

                window.scrollTo(0, 10000000);

                if (typeof self.pageYOffset != 'undefined')
                    OffWindowH = self.pageYOffset;
                else if (document.compatMode && document.compatMode != 'BackCompat')
                    OffWindowH = document.documentElement.scrollTop;
                else if (document.body && typeof (document.body.scrollTop) != 'undefined')
                    OffWindowH = document.body.scrollTop;
                window.scrollTo(0, 0);

                return OffWindowH;
            }

            function WindowHeight() {
                var WindowHeight = 0;
                if (typeof (window.innerWidth) == 'number')
                    WindowHeight = window.innerHeight;
                else if (document.documentElement && document.documentElement.clientHeight)
                    WindowHeight = document.documentElement.clientHeight;
                else if (document.body && document.body.clientHeight)
                    WindowHeight = document.body.clientHeight;

                return WindowHeight;
            }

            function pHeight() {
                var pHeight = OffWindowH() + WindowHeight();
                return pHeight;
            }
            function ModelDialog() {
                var Modal = document.getElementById("modal");
                if (Modal == null) {
                    Modal = document.createElement("div");
                    Modal.setAttribute("id", "modal");
                    Modal.setAttribute("class", "modal");
                    document.body.appendChild(Modal);
                }
                Modal.style.height = pHeight() + 'px';
                Modal.style.display = "block";
            }
            function Loading() {
                var Element = document.getElementById("loading");
                objh = parseFloat(Element.style.height) / 2;
                objw = parseFloat(Element.style.width) / 2;
                Element.style.top = Math.floor(Math.round((document.documentElement.offsetHeight / 2) + document.documentElement.scrollTop) - objh) + 'px';
                Element.style.left = Math.floor(Math.round((document.documentElement.offsetWidth / 2) + document.documentElement.scrollLeft) - objw) + 'px';
                Element.style.display = "block";
            }
            function ShowProgress() {
                ModelDialog();
                Loading();
            }
            function HideProgress() {
                var loading = document.getElementById("loading");
                loading.style.display = "none";
                var modal = document.getElementById("modal");
                if (modal != null)
                    modal.style.display = "none";
            }
        </script>
        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
            prm.add_beginRequest(BeginRequestHandler);
            // Raised after an asynchronous postback is finished and control has been returned to the browser.
            prm.add_endRequest(EndRequestHandler);
            function BeginRequestHandler(sender, args) {
                ShowProgress()
            }

            function EndRequestHandler(sender, args) {
                HideProgress()
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <%-- <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="92%">--%>

        <div id="loading" class="loading" style="width: 200px; height: 100px; line-height: 9em" align="center">
            <asp:Image ID="imgProgress" runat="server" ImageUrl="<%$ PhoenixTheme:images/uploading.gif %>" />
            Processing. Please wait.                                            
        </div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>

        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>

        <eluc:TabStrip ID="MenuPost" runat="server" OnTabStripCommand="MenuPost_TabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="ShowExcel_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server"
            DecorationZoneID="gvCaptainPettyCash" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCaptainPettyCash" runat="server"
            Height="82%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" ShowFooter="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvCaptainPettyCash_NeedDataSource"
            OnItemDataBound="gvCaptainPettyCash_ItemDataBound"
            EnablePartialRendering="false" OnCustomAggregate="gvCaptainPettyCash_CustomAggregate"
            EnableHeaderContextMenu="true" AutoGenerateColumns="false"
            OnRowCreated="RowCreated">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView ShowGroupFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true"
                ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center"
                TableLayout="Fixed">
                <HeaderStyle Width="102px" />
                <GroupByExpressions>
                    <telerik:GridGroupByExpression>
                        <SelectFields>
                            <telerik:GridGroupByField FieldName="FLDTYPENAME" FieldAlias="Details" SortOrder="Ascending" />
                        </SelectFields>
                        <GroupByFields>
                            <telerik:GridGroupByField FieldName="FLDTYPENAME" SortOrder="Ascending" />
                        </GroupByFields>
                    </telerik:GridGroupByExpression>
                </GroupByExpressions>
                <Columns>
                    <telerik:GridTemplateColumn Visible="false" UniqueName="Data" DataField="FLDTYPENAME">
                        <HeaderStyle Width="4%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTYPENAME"]%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component Type">
                        <HeaderStyle Width="50%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblType" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTYPENAME"]%>' Visible="false"></asp:Label>
                            <%# ((DataRowView)Container.DataItem)["FLDLOGTYPENAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Budget Code" AllowSorting="false"
                        ShowSortIcon="true" UniqueName="SUBTOTAL" Aggregate="Custom" FooterStyle-Font-Bold="true"
                        FooterAggregateFormatString="Sub Total">
                        <HeaderStyle Width="18%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDBUDGETCODE"]%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Debit" AllowSorting="false" HeaderStyle-Width="65px"
                        ShowSortIcon="true" UniqueName="DEBIT" DataField="FLDDEBIT" Aggregate="Sum" FooterAggregateFormatString="{0}">
                        <HeaderStyle Width="15%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDDEBIT"]%>
                        </ItemTemplate>

                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Credit" AllowSorting="false"
                        ShowSortIcon="true" Aggregate="Sum" UniqueName="CREDIT" DataField="FLDCREDIT"
                        FooterAggregateFormatString="{0}">
                        <HeaderStyle Width="15%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDCREDIT"]%>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <%--</telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
