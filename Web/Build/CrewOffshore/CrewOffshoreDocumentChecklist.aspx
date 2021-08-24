<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDocumentChecklist.aspx.cs" Inherits="CrewOffshoreDocumentChecklist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Documents Checklist</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>

    </telerik:RadCodeBlock>


</head>
<body>
    <form id="frmAdditionalDoc" runat="server">

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />

        <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:TabStrip ID="ChecklistMenu" runat="server" OnTabStripCommand="ChecklistMenu_TabStripCommand"></eluc:TabStrip>

        <eluc:TabStrip ID="CrewShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>


        <%--<asp:GridView ID="gvDocumentChecklist" OnRowCommand="gvDocumentChecklist_RowCommand" OnRowDataBound="gvDocumentChecklist_RowDataBound" runat="server"
                    AutoGenerateColumns="False" Font-Size="11px" 
                    Width="100%" CellPadding="3" ShowHeader="true" AllowSorting="true" ShowFooter="false"
                    EnableViewState="true" >
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentChecklist" runat="server" Height="500px" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvDocumentChecklist_NeedDataSource"
            OnItemCommand="gvDocumentChecklist_ItemCommand"
            OnItemDataBound="gvDocumentChecklist_ItemDataBound"
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
                    <telerik:GridTemplateColumn HeaderText="Document Category" HeaderStyle-Width="100px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignonoffId" runat="server" Visible="false" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDocType" runat="server" Visible="false" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDocId" runat="server" Visible="false" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDocumentCategory" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Required Document" HeaderStyle-Width="150px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDocName" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Available Document" HeaderStyle-Width="150px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAvilableDocName" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCUMENTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Certificate No" HeaderStyle-Width="150px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcertno" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Expiry Date" HeaderStyle-Width="75px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblexpdate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Original Received Y/N" HeaderStyle-Width="50px">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                        <ItemTemplate>
                            <asp:CheckBox ID="chkOriginalReceivedYN" runat="server" AutoPostBack="true" OnCheckedChanged="Update_Checklist" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDORIGINALRECEIVEDYN")).ToString().Equals("1")?true:false %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Photocopy Received Y/N" HeaderStyle-Width="50px">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                        <ItemTemplate>
                            <asp:CheckBox ID="chkPhotocopyReceivedYN" runat="server" AutoPostBack="true" OnCheckedChanged="Update_Checklist" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDPHOTOCOPYRECEIVEDYN")).ToString().Equals("1")?true:false %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="false" SaveScrollPosition="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

        <%--  <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
                CancelText="No" />--%>
    </form>
</body>
</html>
