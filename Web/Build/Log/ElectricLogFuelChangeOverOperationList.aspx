<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogFuelChangeOverOperationList.aspx.cs" Inherits="Log_ElectricLogFuelChangeOverOperationList" %>

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
    <title>Entries in Fuel Change Over</title>
    
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
    </telerik:radcodeblock>
    <style>
        .bold {
            font-weight: bold;
            font-size: large;
            text-align: center;
        }
        .strike-through {
            text-decoration:line-through;
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
    <form id="frmgvCounterUpdate" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <telerik:RadAjaxPanel id="RadAjaxPanel1" runat="server" height="95%">

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="displayNone" />
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
                    </td>  <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                     <td>
                        <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList runat="server" AutoPostBack="true" ID="ddlStatus" OnSelectedIndexChanged="ddl_TextChanged" ></telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenugvCounterUpdate" runat="server" OnTabStripCommand="gvCounterUpdate_TabStripCommand"></eluc:TabStrip>
       
            <telerik:RadGrid RenderMode="Lightweight" ID="gvElogTransaction" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="true"
                CellSpacing="0" GridLines="None" Height="85%" OnGridExporting="gvElogTransaction_GridExporting" 
                OnNeedDataSource="gvElogTransaction_NeedDataSource" 
                OnItemCommand="gvElogTransaction_ItemCommand" 
                OnItemDataBound="gvElogTransaction_ItemDataBound"
                OnPreRender="gvElogTransaction_PreRender"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText=""  HeaderStyle-Width="25PX">

                                    <ItemStyle HorizontalAlign="Center" />

                                    <ItemTemplate>

                                        <telerik:RadLabel ID="radisbackdatedentry" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISMISSEDENTRY")%>'>
                                        </telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false" >
                                         <span class="icon" id="imgFlagcolor"  ><i class="fas fa-star-yellow"></i></span>      
                                            </asp:LinkButton>
                                         
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span class="icon" style="vertical-align:middle"><i class="fas fa-star-yellow"></i> </span> <b style="vertical-align:middle">* Missed Entry</b>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLogBookId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSACTIONID") %>'></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="Delete" 
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" 
                                    CommandName="VIEW" ID="CmdView"
                                    ToolTip="Edit Log" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
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
                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" ScrollHeight="350px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
            </ClientSettings>
            </telerik:RadGrid>
             <table>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="txtChiefEnggSign" CssClass="signature"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblChiefEnggSign" Text="Signature of CE"></telerik:RadLabel>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>