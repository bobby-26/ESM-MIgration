<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaGenerationReport.aspx.cs"
    Inherits="AccountsSoaGenerationReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Statement of Accounts</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            var Template = new Array();
            Template[0] = '<a href="#" id="lnkRemoveFile{counter}" onclick="return removeFile(this);">Remove</a>';
            Template[1] = '<input id="txtFileUpload{counter}" name="txtFileUpload{counter}" type="file" class="input" />';
            Template[2] = 'Choose a file';
            var counter = 1;

            function addFile(description) {
                counter++;
                var tbl = document.getElementById("tblFiles");
                var rowCount = tbl.rows.length;
                var row = tbl.insertRow(rowCount - 1);
                var cell;

                for (var i = 0; i < Template.length; i++) {
                    cell = row.insertCell(0);
                    cell.innerHTML = Template[i].replace(/\{counter\}/g, counter).replace(/\{value\}/g, (description == null) ? '' : description);
                }
            }
            function removeFile(ctrl) {
                var tbl = document.getElementById("tblFiles");
                if (tbl.rows.length > 2)
                    tbl.deleteRow(ctrl.parentNode.parentNode.rowIndex);
            }


        </script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
            }
        }
            function confirmucConfirmSOAScheduleMsg(args) {
                if (args) {
                    __doPostBack("<%=confirmucConfirmSOAScheduleMsg.UniqueID %>", "");
                }
            }

            function confirmucConfirm2ndlevel(args) {
                if (args) {
                    __doPostBack("<%=confirmucConfirm2ndlevel.UniqueID %>", "");
                }
            }

            function confirmucConfirmpublish(args) {
                if (args) {
                    __doPostBack("<%=confirmucConfirmpublish.UniqueID %>", "");
                }
            }
            function confirmucConfirmAddlreattch(args) {
                if (args) {
                    __doPostBack("<%=confirmucConfirmAddlreattch.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOwnersAccounts" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuGenralSub" runat="server" OnTabStripCommand="MenuGenralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuSOALineItems" runat="server" OnTabStripCommand="MenuSOALineItems_TabStripCommand"></eluc:TabStrip>


            <%-- <eluc:TabStrip ID="MenuVoucherLI" runat="server" OnTabStripCommand="MenuVoucherLI_TabStripCommand"></eluc:TabStrip>--%>

            <telerik:RadLabel ID="lblSOA" runat="server" Font-Bold="true" ForeColor="Blue"></telerik:RadLabel>
            <table width="100%">
                <tr>
                    <td style="vertical-align: top; min-height: 850px;">
                        <table width="100%">
                            <tr>
                                <td style="width: 100%;" valign="top" align="left">
                                    <asp:Panel ID="pnlReportsGeneration" runat="server" GroupingText="Reports Generation" Height="150px" ScrollBars="Auto">
                                        <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                                            <%-- <asp:GridView ID="gvOwnersAccount" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                CellPadding="3" AllowSorting="true" ShowHeader="true" EnableViewState="false"
                                                OnRowCommand="gvOwnersAccount_RowCommand">
                                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                                <RowStyle Height="10px" />--%>
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnersAccount" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOwnersAccount_NeedDataSource"
                                                OnItemCommand="gvOwnersAccount_ItemCommand"
                                                GroupingEnabled="false" EnableHeaderContextMenu="true"
                                                AutoGenerateColumns="false">
                                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                    AutoGenerateColumns="false" TableLayout="Fixed">
                                                    <NoRecordsTemplate>
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </NoRecordsTemplate>
                                                    <HeaderStyle Width="102px" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Report Name">

                                                            <ItemTemplate>
                                                                <%--<telerik:RadLabel ID="lblReportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>'></telerik:RadLabel>--%>
                                                                <asp:LinkButton ID="lblReportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>' CommandName="Report" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                                                <telerik:RadLabel ID="lblReportCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBREPORTCODE") %>' Visible="false"></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Verified" HeaderStyle-Width="250px">
                                                            <ItemStyle HorizontalAlign="Left" />

                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblVerified" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDDETAILS") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                                                </MasterTableView>
                                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                </ClientSettings>
                                            </telerik:RadGrid>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%;" valign="top" align="left">
                                    <asp:Panel ID="pnlGeneratedReports" runat="server" GroupingText="Combined PDF" Height="80px" ScrollBars="Auto">
                                        <div id="dvGeneratedReports" style="position: relative; z-index: 1; width: 100%;">
                                            <%-- <asp:GridView ID="gvCombinedPDF" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                CellPadding="3" AllowSorting="true" ShowHeader="true" EnableViewState="false"
                                                OnRowCommand="gvCombinedPDF_RowCommand" OnRowUpdating="gvCombinedPDF_RowUpdating"
                                                OnRowEditing="gvCombinedPDF_RowEditing" OnRowCancelingEdit="gvCombinedPDF_RowCancelingEdit">
                                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                                <RowStyle Height="10px" />--%>
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gvCombinedPDF" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCombinedPDF_NeedDataSource"
                                                OnItemCommand="gvCombinedPDF_ItemCommand"
                                                GroupingEnabled="false" EnableHeaderContextMenu="true"
                                                AutoGenerateColumns="false">
                                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                    AutoGenerateColumns="false" TableLayout="Fixed">
                                                    <NoRecordsTemplate>
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </NoRecordsTemplate>
                                                    <HeaderStyle Width="102px" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="CombinedPDF">

                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCombinedODF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>' CommandName="PDFView" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Created Date">

                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMBINEDPDFGENDATE") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="URL">
                                                            <ItemStyle HorizontalAlign="Right" />

                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblURL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <telerik:RadTextBox ID="txtURL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadTextBox>
                                                            </EditItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="100px">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                                                    ToolTip="Edit"></asp:ImageButton>
                                                                <img id="Img8" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                                    width="3" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                                                    ToolTip="Save"></asp:ImageButton>
                                                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                                    width="3" />
                                                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                                                    ToolTip="Cancel"></asp:ImageButton>
                                                            </EditItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                                                </MasterTableView>
                                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                </ClientSettings>
                                            </telerik:RadGrid>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%;" valign="top" align="left">
                                    <asp:Panel ID="Panel2" runat="server" GroupingText="Owner Portal" Height="80px" ScrollBars="Auto">
                                        <div id="divOwnerReports" style="position: relative; z-index: 1; width: 100%;">
                                            <%--<asp:GridView ID="gvOwner" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                Width="100%" CellPadding="3" AllowSorting="true" ShowHeader="true" EnableViewState="false"
                                                OnRowDataBound="gvOwner_RowDataBound" OnRowCommand="gvOwner_RowCommand">
                                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                                <RowStyle Height="10px" />--%>
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gvOwner" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOwner_NeedDataSource"
                                                OnItemCommand="gvOwner_ItemCommand"
                                                OnItemDataBound="gvOwner_ItemDataBound"
                                                GroupingEnabled="false" EnableHeaderContextMenu="true"
                                                AutoGenerateColumns="false">
                                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                    AutoGenerateColumns="false" TableLayout="Fixed">
                                                    <NoRecordsTemplate>
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </NoRecordsTemplate>
                                                    <HeaderStyle Width="102px" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Account Name" HeaderStyle-Width="25%">

                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblOwnerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblAccountName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn HeaderText="SOA Reference" HeaderStyle-Width="15%">

                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblAccountCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblSoaReference" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblURL" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
                                                                <asp:LinkButton ID="lnkSoaReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'
                                                                    CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                                                <telerik:RadLabel ID="lblSoaReferenceid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEID") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblAsonDate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASONDATE") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Month" HeaderStyle-Width="5%">

                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblMonthId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Year" HeaderStyle-Width="5%">

                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblYearId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50%">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <a id="aEPSSHelp" href='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' style="color: blue;" target="_blank" fore-color="" runat="server">SOA</a>
                                                                <%--                                    <asp:LinkButton runat="server" AlternateText="SOA" Text = "SOA"
                                        CommandName="SOAPDF" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSOA"
                                        ToolTip="SOA"></asp:LinkButton>--%>
                                                                <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                <asp:LinkButton runat="server" AlternateText="Vessel Trial Balance" Text="TB"
                                                                    CommandName="TBPDF" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdTBPdf"
                                                                    ToolTip="Vessel Trial Balance" Visible="false"></asp:LinkButton>
                                                                <img id="Img9" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                <asp:LinkButton runat="server" AlternateText="Vessel Trial Balance" Text="TB"
                                                                    CommandName="TBYTDPDF" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdTBYTD"
                                                                    ToolTip="Vessel Trial Balance" Visible="false"></asp:LinkButton>
                                                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                <asp:LinkButton runat="server" AlternateText="Vessel Trial Balance" Text="TB"
                                                                    CommandName="TBYTDOWNPDF" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdTBYTDOwner"
                                                                    ToolTip="Vessel Trial Balance" Visible="false"></asp:LinkButton>
                                                                <img id="Img10" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                <asp:LinkButton runat="server" AlternateText="Vessel Summary Balance" Text="SUMMARY"
                                                                    CommandName="summaryDF" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSummaryPdf"
                                                                    ToolTip="Vessel Summary Balance" Visible="false"></asp:LinkButton>
                                                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                <asp:ImageButton runat="server" AlternateText="Excel" ImageUrl="<%$ PhoenixTheme:images/xls.png %>"
                                                                    CommandName="EXCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdExcel"
                                                                    ToolTip="Excel"></asp:ImageButton>
                                                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                                                    ToolTip="Attachment"></asp:ImageButton>
                                                                <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                <asp:LinkButton runat="server" AlternateText="MonthlyVariance" Text="Monthly" CommandName="MONTHLYVARIANCE" CommandArgument='<%# Container.DataSetIndex %>'
                                                                    ID="cmdMonthlyVariance" ToolTip="Monthly Variance" Visible="false">
                                                                </asp:LinkButton>
                                                                <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                <asp:LinkButton runat="server" AlternateText="Yearly Variance" Text="Yearly" CommandName="YEARLYVARIANCE" CommandArgument='<%# Container.DataSetIndex %>'
                                                                    ID="cmdYearlyVariance" ToolTip="Yearly Variance" Visible="false"></asp:LinkButton>
                                                                <img id="Img7" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                                <asp:LinkButton runat="server" AlternateText="Accumulated Variance" Text="Accumulated" CommandName="ACCUMALTEDVARIANCE" CommandArgument='<%# Container.DataSetIndex %>'
                                                                    ID="cmdAccumaltedVariance" ToolTip="Accumulated Variance" Visible="false">
                                                                </asp:LinkButton>
                                                                <asp:ImageButton runat="server" AlternateText="Funds Position" ImageUrl="<%$ PhoenixTheme:images/pdf.png  %>"
                                                                    CommandName="PDF" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPdf"
                                                                    ToolTip="Funds Position" Visible="false"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                                                </MasterTableView>
                                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                </ClientSettings>
                                            </telerik:RadGrid>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%;" valign="top" align="left">
                                    <asp:Panel ID="pnlManualReports" runat="server" GroupingText="Manual Reports" Height="50px" ScrollBars="Auto">
                                        <div id="divManReports" style="position: relative; z-index: 1; width: 100%;">
                                            <%-- <asp:GridView ID="gvManualReports" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                CellPadding="3" AllowSorting="true" ShowHeader="true" EnableViewState="false"
                                                OnRowCommand="gvManualReports_RowCommand">
                                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                                <RowStyle Height="10px" />--%>
                                            <telerik:RadGrid RenderMode="Lightweight" ID="gvManualReports" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvManualReports_NeedDataSource"
                                                OnItemCommand="gvManualReports_ItemCommand"
                                                GroupingEnabled="false" EnableHeaderContextMenu="true"
                                                AutoGenerateColumns="false">
                                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                    AutoGenerateColumns="false" TableLayout="Fixed">
                                                    <NoRecordsTemplate>
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </NoRecordsTemplate>
                                                    <HeaderStyle Width="102px" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Report Name">

                                                            <ItemTemplate>
                                                                <%--<telerik:RadLabel ID="lblReportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTNAME") %>'></telerik:RadLabel>--%>
                                                                <asp:LinkButton ID="lblReportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBREPORTTYPE") %>' CommandName="Report"
                                                                    CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                                                <telerik:RadLabel ID="lblReportCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBREPORTCODE") %>' Visible="false"></telerik:RadLabel>
                                                                <telerik:RadLabel ID="lblDTKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANUALREPORTDTKEY") %>' Visible="false"></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn Visible="false">
                                                            <HeaderTemplate>
                                                                <asp:Literal ID="lblAction" runat="server" Text="Verify"></asp:Literal>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgManualVerify" runat="server" ImageUrl="~/css/Theme1/images/45.png" AlternateText="Verify" ToolTip="Verify" CommandName="VERIFY" CommandArgument='<%# Container.DataSetIndex %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                                                </MasterTableView>
                                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                </ClientSettings>
                                            </telerik:RadGrid>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%;" valign="top" align="left">
                                    <div class="navigation" id="navigation1" style="width: 99%; height: 25px; z-index: 1">
                                        <%--top: 0px; margin-left: 0px; vertical-align: top; --%>
                                        <div class="subHeader">
                                            <eluc:Title runat="server" ID="Attachment" Text="Attachment" ShowMenu="false"></eluc:Title>
                                        </div>

                                        <eluc:TabStrip ID="AttachmentList" runat="server" OnTabStripCommand="AttachmentList_TabStripCommand"></eluc:TabStrip>

                                        <table id="tblFiles">
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="lblChooseafile" runat="server" Text="Choose a file"></asp:Literal>
                                                </td>
                                                <td colspan="2">
                                                    <asp:FileUpload ID="txtFileUpload1" runat="server" CssClass="input" />
                                                </td>
                                                <td>
                                                    <telerik:RadLabel ID="lblStatus" runat="server" Visible="false">Status</telerik:RadLabel>
                                                </td>
                                                <td>
                                                    <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="49"
                                                        CssClass="input_mandatory" Visible="false" ShortNameFilter="APP,NAP,CAP" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="right">
                                                    <a href="#" onclick="return addFile();">Add File</a>
                                                </td>
                                            </tr>
                                        </table>
                                        <hr />
                                        <br />

                                        <eluc:TabStrip ID="SubMenuAttachment" runat="server"></eluc:TabStrip>

                                        <%-- <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowUpdating="gvAttachment_RowUpdating"
                                            OnRowCancelingEdit="gvAttachment_RowCancelingEdit" OnRowDataBound="gvAttachment_RowDataBound"
                                            OnRowEditing="gvAttachment_RowEditing" OnRowDeleting="gvAttachment_RowDeleting"
                                            AllowSorting="true" OnSorting="gvAttachment_Sorting">
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                            <RowStyle Height="10px" />--%>
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvAttachment_NeedDataSource"
                                            OnItemCommand="gvAttachment_ItemCommand"
                                            OnItemDataBound="gvAttachment_ItemDataBound"
                                            OnUpdateCommand="gvAttachment_UpdateCommand"
                                            OnDeleteCommand="gvAttachment_DeleteCommand"
                                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                                            AutoGenerateColumns="false">
                                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                AutoGenerateColumns="false" TableLayout="Fixed">
                                                <NoRecordsTemplate>
                                                    <table width="100%" border="0">
                                                        <tr>
                                                            <td align="center">
                                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
                                                <HeaderStyle Width="102px" />
                                                <Columns>

                                                    <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="5%">

                                                        <ItemTemplate>
                                                            <asp:Image ID="imgfiletype" runat="server" Width="14px" Height="14px" />
                                                            <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="File Name" HeaderStyle-Width="30%" AllowSorting="true" SortExpression="FLDFILENAME">
                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>

                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="30%">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                                                Height="14px" ToolTip="Download File">
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Size(in KB)" HeaderStyle-Width="10%" SortExpression="FLDFILESIZE" AllowSorting="true">
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>

                                                        <ItemTemplate>
                                                            <%# string.IsNullOrEmpty(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString()) ? string.Empty : Math.Round(((double.Parse(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString())/1024*100000)/100000.00),2).ToString()%>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Synch(Yes/No)" HeaderStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblsynchyesno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <telerik:RadLabel ID="lblDTKeyEdit" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></telerik:RadLabel>
                                                            <asp:CheckBox ID="chkSynch" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))? true:false %>' />
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Created By" HeaderStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblCreatedBy" runat="server" Text='<%#Bind("FLDCREATEDBYNAME") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Created Date" HeaderStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lbldate" runat="server" Text='<%#Bind("FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="30%" HeaderText="Action">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                                                ToolTip="Edit"></asp:ImageButton>
                                                            <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                                CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                                                ToolTip="Delete"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                                CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                                                ToolTip="Save"></asp:ImageButton>
                                                            <img runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                                                ToolTip="Cancel"></asp:ImageButton>
                                                        </EditItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>

                                        <eluc:Splitter ID="ucSplitter" runat="server" TargetControlID="ifMoreInfo" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <%--<eluc:Confirm ID="ucConfirmMsg" runat="server" Visible="false" OKText="Yes" CancelText="No" OnConfirmMesage="CheckMapping_Click" />--%>
            <%-- <eluc:Confirm ID="ucConfirmSOAScheduleMsg" runat="server" Visible="false" OKText="Yes" CancelText="No" OnConfirmMesage="SOASchedule_Click" />
                <eluc:Confirm ID="ucConfirm2ndlevel" runat="server" Visible="false" OKText="Yes" CancelText="No" OnConfirmMesage="ucConfirm2ndlevel_Click" />
                <eluc:Confirm ID="ucConfirmpublish" runat="server" Visible="false" OKText="Yes" CancelText="No" OnConfirmMesage="ucConfirmpublish_Click" />
                <eluc:Confirm ID="ucConfirmAddlreattch" runat="server" Visible="false" OKText="Yes" CancelText="No" OnConfirmMesage="ucConfirmAddlreattch_Click" />--%>

            <asp:Button runat="server" ID="confirm" CssClass="hidden" OnClick="CheckMapping_Click" />
            <telerik:RadWindowManager runat="server" ID="ucConfirmMsg" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            <asp:Button runat="server" ID="confirmucConfirmSOAScheduleMsg"  CssClass="hidden" OnClick="SOASchedule_Click" />
            <telerik:RadWindowManager runat="server" ID="ucConfirmSOAScheduleMsg" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            <asp:Button runat="server" ID="confirmucConfirm2ndlevel" CssClass="hidden" OnClick="ucConfirm2ndlevel_Click" />
            <telerik:RadWindowManager runat="server" ID="ucConfirm2ndlevel" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            <asp:Button runat="server" ID="confirmucConfirmpublish"  CssClass="hidden" OnClick="ucConfirmpublish_Click" />
            <telerik:RadWindowManager runat="server" ID="ucConfirmpublish" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            <asp:Button runat="server" ID="confirmucConfirmAddlreattch" CssClass="hidden" OnClick="ucConfirmAddlreattch_Click" />
            <telerik:RadWindowManager runat="server" ID="ucConfirmAddlreattch" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        
          

    </form>
</body>
</html>
