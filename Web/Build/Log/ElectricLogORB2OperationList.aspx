<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2OperationList.aspx.cs" Inherits="Log_ElectricLogORB2OperationList" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Entries in Oil Record Book Part - 2 </title>
    <%--<title></title>--%>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
    </telerik:RadCodeBlock>
    <style>
        .bold {
            font-weight: bold;
            font-size: large;
            text-align: center;
        }

        .strike-through {
            text-decoration: line-through;
        }

        .signature {
            float: left;
            text-decoration: underline;
            font-size: 16px;
            font-weight: bold;
        }

        .displayNone {
            display: none;
        }

        .fa-unlock {
            background-color: red;
        }

        .fa-lock {
            background-color: green;
        }

        .not-signed {
            background-color: #ffc200;
            width: 250px;
            display: inline-block;
        }
        .user-info {
            float:right;
        }
    </style>
    <script>

        document.addEventListener("DOMContentLoaded", function () {
            pageOnLoad();
        });


        document.addEventListener("load", function () {
            pageOnLoad();
        });

        function pageOnUnload() {

        }

        function pageOnLoad() {
            $('.rgPagerTextBox').attr('readonly', true);
            $('.rgPagerButton').css('display', 'none');
        }
    </script>
</head>
<body>
    <form id="frmgvCounterUpdate" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="82%" ClientEvents-OnRequestStart="pageOnUnload" ClientEvents-OnResponseEnd="pageOnLoad">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

            <table class="user-info">
                <tr>
                    <td><telerik:RadLabel runat="server" ID="lblUsername"></telerik:RadLabel></td>
                </tr>
            </table>

            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadComboBox DropDownPosition="Static" style="width:auto" AutoPostBack="true" ID="ddlStatus" runat="server"  EnableLoadOnDemand="True"
                            OnTextChanged="ddl_TextChanged"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>--%>
                        <telerik:RadDropDownList runat="server" AutoPostBack="true" Width="180px" ID="ddlStatus" OnItemSelected="ddl_TextChanged"></telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <%--<div style="width: 100%; text-align: right">
                <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" ImageUrl="<%$ PhoenixTheme:images/54.png %>" Width="14px"
                    Height="14px" ToolTip="User Guide">
                </asp:HyperLink>
            </div>--%>
            <eluc:TabStrip ID="MenugvCounterUpdate" runat="server" OnTabStripCommand="gvCounterUpdate_TabStripCommand"></eluc:TabStrip>


            <telerik:RadGrid RenderMode="Lightweight" ID="gvElogTransaction" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="true"
                CellSpacing="0" GridLines="None" Height="90%" OnGridExporting="gvElogTransaction_GridExporting"
                OnNeedDataSource="gvElogTransaction_NeedDataSource"
                OnItemCommand="gvElogTransaction_ItemCommand"
                OnItemDataBound="gvElogTransaction_ItemDataBound"
                OnPreRender="gvElogTransaction_PreRender"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName="MarpolLog" ExportOnlyData="true">
                    <Pdf PageHeight="210mm" PageWidth="297mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="45mm"
                        BorderStyle="Medium" BorderColor="#666666">
                    </Pdf>
                </ExportSettings>
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="ROB (m3)" HeaderStyle-Width="150px" Name="FromROB" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="ROB (m3)" Name="ToROB" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>

                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Date" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDDATE" UniqueName="date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                            <ItemTemplate>
                                <%--<telerik:RadLabel ID="RadLabel1" runat="server" Visible="True" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDATE",  "{0:dd-MMM-yyyy}") %>'></telerik:RadLabel>--%>
                                <telerik:RadLabel ID="lblDate" runat="server" Visible="True" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERNO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogBookId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGBOOKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTxnId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTXID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLogName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Code" HeaderStyle-Width="100px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCODE" UniqueName="code">
                            <ItemStyle Wrap="true" HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Item No." HeaderStyle-Width="80px" UniqueName="itemNo">
                            <ItemStyle Wrap="true" VerticalAlign="Middle" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblItemNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMNUMBER") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Record of Operations/ Signature of Officer Incharge" HeaderStyle-Width="400px" UniqueName="record">
                            <ItemStyle Wrap="true" VerticalAlign="Middle" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>

                                <div runat="server" id="recordContainer" style="width: 550px; vertical-align: middle;">
                                    <telerik:RadLabel ID="lblRecord" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECORD") %>'></telerik:RadLabel>

                                    <div style="float: right;">
                                        <asp:LinkButton runat="server" AlternateText="Incharge Signature" Visible="false"
                                            CommandName="INCHARGESIGNATURE" ID="btnInchargeSign"
                                            ToolTip="Incharge Signature" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cheif Engineer Signature" Visible="false"
                                            CommandName="CHEIFENGINEERSIGNATURE" ID="btnCheifEngineerSignature"
                                            ToolTip="Chief Engineer Signature" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                        </asp:LinkButton>
                                    </div>
                                    <div style="float: right; width: 250px; vertical-align: middle;">
                                        <div class="deleted">
                                            <telerik:RadLabel ID="lblDeleted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrentStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Center" UniqueName="action">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" Visible="false"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" Visible="false"
                                    CommandName="VIEW" ID="CmdView"
                                    ToolTip="Edit Log" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Attachment" Visible="false"
                                    CommandName="ATTACHMENT" ID="cmdAttachment"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                                    <span class="icon"><i runat="server" id="attachmentIcon" class="fas fa-paperclip-na"></i></span>
                                </asp:LinkButton>

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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" ScrollHeight="350px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="txtMasterSign" CssClass="signature"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblMasterSign" Text="Signature of Master"></telerik:RadLabel>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

