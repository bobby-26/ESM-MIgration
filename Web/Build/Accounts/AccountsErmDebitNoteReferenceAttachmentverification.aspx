<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsErmDebitNoteReferenceAttachmentverification.aspx.cs"
    Inherits="Accounts_AccountsErmDebitNoteReferenceAttachmentverification" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Country</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
          <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvDebitReference.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
        </script>

    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmErmDebitReference" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:Title runat="server" ID="ucTitle" Text="Attachments Verification" ShowMenu="false"></eluc:Title>

            <%--                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuFormMain" runat="server" OnTabStripCommand="MenuFormMain_TabStripCommand">
                        </eluc:TabStrip>
                    </div>--%>
            <br />
            <div id="divFind" runat="server">
                <table>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblyear" runat="server" Text="Year"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Year ID="ucYear" runat="server" AppendDataBoundItems="true"
                                QuickTypeCode="55" Width="80px" />
                        </td>

                        <td style="width: 100px"></td>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Principal"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPickListMaker">
                                <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="false"
                                    Width="200px">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="ImgSupplierPickList" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    Style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                                <telerik:RadTextBox ID="txtVenderName" runat="server" ReadOnly="false"
                                    Width="180px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                            </span>

                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblmonth" runat="server" Text="Month"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Month ID="ucMonth" runat="server" AppendDataBoundItems="true" Width="80px"
                                HardTypeCode="55" SortByShortName="true" />
                        </td>
                    </tr>

                </table>
            </div>
            <br />

            <eluc:TabStrip ID="MenuDebitReference" runat="server" OnTabStripCommand="MenuDebitReference_TabStripCommand"></eluc:TabStrip>


            <%--  <asp:GridView ID="gvDebitReference" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvDebitReference_RowCommand"
                    OnRowDataBound="gvDebitReference_ItemDataBound" OnRowCancelingEdit="gvDebitReference_RowCancelingEdit"
                    OnSelectedIndexChanging="gvDebitReference_SelectedIndexChanging" OnRowDeleting="gvDebitReference_RowDeleting"
                    OnRowUpdating="gvDebitReference_RowUpdating" OnRowEditing="gvDebitReference_RowEditing"
                    ShowHeader="true" EnableViewState="false" AllowSorting="true"
                    OnSorting="gvDebitReference_Sorting" DataKeyNames="FLDDEBINOTEREFERENCEDTKEY">

                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDebitReference" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvDebitReference_NeedDataSource"
                OnItemCommand="gvDebitReference_ItemCommand"
                OnItemDataBound="gvDebitReference_ItemDataBound1"
                OnSortCommand="gvDebitReference_SortCommand"
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
                     
                        <telerik:GridTemplateColumn HeaderText="ID" HeaderStyle-Width="150px">

                            <itemtemplate>
                                <telerik:RadLabel ID="lblDebitnotedtkey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEDTKEY") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblDebitnoteid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsPhoenixAttachment" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPHOENIXATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFilename" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAtttachmentDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILEATTACHMENTDTKEY") %>'></telerik:RadLabel>
                            </itemtemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Month">
                         
                            <itemtemplate>
                                <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHNAME") %>'></telerik:RadLabel>
                            </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Year">
 
                            <itemtemplate>
                                <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></telerik:RadLabel>
                            </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Principal">

                            <itemtemplate>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPAL") %>'></telerik:RadLabel>
                            </itemtemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                      
                            <itemstyle wrap="False" horizontalalign="Center" ></itemstyle>
                            <itemtemplate>
                                <asp:LinkButton runat="server" AlternateText="View" Text="View" CommandName="ViewAttachment" CommandArgument='<%# Container.DataSetIndex %>'
                                    ID="cmdViewAttachment" ToolTip="View Attachment"> </asp:LinkButton>
                            </itemtemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>



        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
