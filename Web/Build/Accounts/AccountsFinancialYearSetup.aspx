<%@ Page Language="C#" AutoEventWireup="True" CodeFile="AccountsFinancialYearSetup.aspx.cs"
    Inherits="AccountsFinancialYearSetup" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Financial Year Setup</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            body, html {
                width: 100%;
                height: 100%;
                margin: 0px;
                padding: 0px;
                background-color: #FFFFFF;
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
            var fileDownloadCheckTimer = null

            function ExportInvoiceAccurals(finyear, usercode) {
                document.cookie = "fileDownloadToken=" + new Date() + ";path=/";
                ShowProgress();
                fileDownloadCheckTimer = setInterval(function () { ClearInterval() }, 100);
                document.getElementById("ifrGeneratedFile").src = "../Options/OptionsAccounts.ashx?methodname=INVOICEACCURALS&finyear=" + finyear + "&usercode=" + usercode;
            }
            function getCookie(cname) {
                var name = cname + "=";
                var ca = document.cookie.split(';');
                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) == ' ') c = c.substring(1);
                    if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
                }
                return "";
            }
            function ClearInterval() {
                ShowProgress();
                if (getCookie("fileDownloadToken") == "") {
                    HideProgress();
                    window.clearInterval(fileDownloadCheckTimer);
                }
            }
            function GridBind() {
                document.getElementById("cmdHiddenSubmit").click();
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
                HideProgress();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAccountsFinancialYearSetup" runat="server">
        <telerik:RadScriptManager runat="server" ID="ToolkitScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlERMOtherSubAccount" Height="100%" EnableAJAX="true">
            <%--<iframe runat="server" id="ifrGeneratedFile" src="about:blank" width="0" height="0"
                    style="display: none" />
                <div id="loading" class="loading" style="width: 200px; height: 100px; line-height: 9em"
                    align="center">
                    <asp:Image ID="imgProgress" runat="server" ImageUrl="<%$ PhoenixTheme:images/uploading.gif %>" />
                    Processing. Please wait.
                </div> --%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucSatus" runat="server" Text="" />

            <eluc:TabStrip ID="MenuPeriodLock" runat="server" OnTabStripCommand="FinancialYearSetup_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <br />
            <table id="tblFinancialYearSetup" width="25%">
                <tr>
                    <%--                            <td>
                                <asp:Literal ID="lblCompanyName" runat="server" Text="Company Name"></asp:Literal>
                            </td>
                            <td>
                                <%=strCompanynamedisplay%>
                                &nbsp;
                            </td>--%>
                    <td>
                        <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Financial Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFinancialYear" runat="server" MaxLength="4" CssClass="input" Width="60px"></telerik:RadTextBox>

                        <%-- <eluc:Number ID="txtFinancialYear" runat="server" CssClass="input_mandatory" MaxValue="9999" IsPositive="true" />
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditFinancialYear" runat="server" AutoComplete="false"
                                    InputDirection="RightToLeft" Mask="9999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtFinancialYear" /> --%>  
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuFinancialYearSetup" runat="server" OnTabStripCommand="FinancialYearSetup_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="dgFinancialYearSetup" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="dgFinancialYearSetup_ItemCommand" OnItemDataBound="dgFinancialYearSetup_ItemDataBound"
                AllowPaging="true" AllowCustomPaging="true" Height="77%" AllowSorting="true" EnableViewState="false" ShowFooter="true"
                OnNeedDataSource="dgFinancialYearSetup_NeedDataSource" OnSortCommand="dgFinancialYearSetup_Sorting"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Company Name" AllowSorting="true" HeaderStyle-Width="234px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="234px"></ItemStyle>
                            <HeaderStyle Width="234px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompanyID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFinancialYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEAR") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Financial Start Year" AllowSorting="true" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFinancialStartYear" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALSTARTYEAR","{0:dd/MMM/yyyy}") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:UserControlDate ID="txtFinancialStartYearAdd" runat="server" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Financial End Year" AllowSorting="true" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="110px"></ItemStyle>
                            <HeaderStyle Width="110px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkFinancialEndYear" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALENDYEAR","{0:dd/MMM/yyyy}") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:UserControlDate ID="txtFinancialEndYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALENDYEAR","{0:dd/MMM/yyyy}") %>'
                                    CssClass="input_mandatory" />
                                <eluc:UserControlDate ID="txtFinancialStartYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALSTARTYEAR","{0:dd/MMM/yyyy}") %>'
                                    CssClass="input_mandatory" Visible="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Financial Year" AllowSorting="true" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="110px"></ItemStyle>
                            <HeaderStyle Width="110px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkFinancialYear" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsRecentFinancialYear" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                    Visible="false" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECENTFINANCIALYEAR") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtFinancialYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'
                                    CssClass="input_mandatory" Width="100%" />
                                <telerik:RadLabel ID="lblIsRecentFinancialYear" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                    Visible="false" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECENTFINANCIALYEAR") %>'>
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtFinancialYear" runat="server" CssClass="input_mandatory" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSTATUS").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtMapCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPCODE") %>'
                                    Visible="false">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtMapCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPCODE") %>'
                                    Visible="false">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtCompanyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'
                                    Visible="false">
                                </telerik:RadTextBox>
                                <asp:CheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSTATUS").ToString().Equals("1"))?true:false %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkActiveYNAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Closed Date" AllowSorting="true" HeaderStyle-Width="86px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="86px"></ItemStyle>
                            <HeaderStyle Width="86px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Closed By" AllowSorting="true" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="110px"></ItemStyle>
                            <HeaderStyle Width="110px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Accounts Transferred" AllowSorting="true" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="75px"></ItemStyle>
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Accounts Transferred" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                    CommandName="AccountsTransferred" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAccountsTransferred"
                                    ToolTip="Accounts Transferred"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vouchers Posted" AllowSorting="true" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="110px"></ItemStyle>
                            <HeaderStyle Width="110px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVouchersposted" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPOSTEDVOUCHERNUMBERS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="135px">
                            <HeaderStyle HorizontalAlign="Center" Width="135px" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="135px"></ItemStyle>
                            <FooterTemplate>
                                <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdAdd" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png%>" ToolTip="Add New" />
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                            <ItemTemplate>
                                <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" />
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete" Visible="false" />
                                <asp:ImageButton ID="cmdExcelExport" runat="server" AlternateText="ExcelExport" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="EXCELEXPORT" ImageUrl="<%$ PhoenixTheme:images/icon_xls.png %>" ToolTip="Invoice Accurals" />
                                <asp:ImageButton ID="cmdPostAccurals" runat="server" AlternateText="PostAccurals" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="POSTACCURALS" ImageUrl="<%$ PhoenixTheme:images/pr.png %>" ToolTip="Post Invoice Accurals" />
                                <asp:ImageButton ID="cmdForexRevaluation" runat="server" AlternateText="PostAccurals" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="FOREXREVALUATION" ImageUrl="<%$ PhoenixTheme:images/pr.png %>" ToolTip="Forex Revaluation" />
                                <asp:ImageButton ID="cmdOpeningBalance" runat="server" AlternateText="PostAccurals" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="OPENINGBALANCE" ImageUrl="<%$ PhoenixTheme:images/pr.png %>" ToolTip="Opening Balance" />
                                <asp:ImageButton ID="cmdUnDo" runat="server" AlternateText="Undo All Posting" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="UNDOPOSTING" ImageUrl="<%$ PhoenixTheme:images/cancel.png %>" ToolTip="Undo Posting" />
                                <asp:ImageButton runat="server" AlternateText="Lock UnLock History" ImageUrl="<%$ PhoenixTheme:images/document_view.png %>"
                                    CommandName="VIEWHISTORY" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdHistory" ToolTip="Lock UnLock History"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
