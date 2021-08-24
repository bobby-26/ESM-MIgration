<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetOpeningBalancesGenerate.aspx.cs" Inherits="CommonBudgetOpeningBalancesGenerate" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Opening Balance Upload</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <%-- <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFinancialYearSetup.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
        </script>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--  <eluc:Title runat="server" ID="Attachment" Text="Opening Balance Upload" ShowMenu="false"></eluc:Title>--%>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="AttachmentList" runat="server" OnTabStripCommand=" MenutravelInvoice_OnTabStripCommand"></eluc:TabStrip>
        <%-- <eluc:TabStrip ID="TabStrip1" runat="server" OnTabStripCommand="AttachmentList_TabStripCommand">
            </eluc:TabStrip>--%>

        <table id="tblFiles">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                </td>
                <td colspan="2">
                    <asp:FileUpload ID="txtFileUpload1" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="lnkDownloadExcel" runat="server" Text="Download Template"
                        OnClick="lnkDownloadExcel_Click"></asp:LinkButton>
                </td>
                <td />
            </tr>

        </table>
        <hr />
        <br />

        <eluc:TabStrip ID="SubMenuAttachment" runat="server"></eluc:TabStrip>

        <eluc:Splitter ID="ucSplitter" runat="server" TargetControlID="ifMoreInfo" />
        <%-- <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowUpdating="gvAttachment_RowUpdating"
            OnRowCommand="gvAttachment_RowCommand" OnRowDataBound="gvAttachment_RowDataBound"
            OnRowDeleting="gvAttachment_RowDeleting" AllowSorting="true">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <RowStyle Height="10px" />--%>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvAttachment_NeedDataSource"
            OnItemCommand="gvAttachment_ItemCommand"
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
                   
                    <telerik:GridTemplateColumn HeaderText="File Name" SortExpression="FLDFILENAME" AllowSorting ="true">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                      
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbluploadedfileid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOPENINGFILEUPLOADID").ToString() %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </telerik:RadLabel>
                            <%--  <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDUPLOADFILPATH").ToString() %>'></telerik:RadLabel>--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Created Date">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbldate" runat="server" Text='<%#Bind("FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>

                            <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Post" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                CommandName="Post" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPost"
                                ToolTip="Post"></asp:ImageButton>
                            <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <eluc:Splitter ID="Splitter1" runat="server" TargetControlID="ifMoreInfo" />

    </form>
</body>
</html>
