<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderLogPTW.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderLogPTW" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PTW</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderRequisition" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadGrid ID="gvPTW" runat="server" RenderMode="Lightweight" OnNeedDataSource="gvPTW_NeedDataSource"
            OnItemDataBound="gvPTW_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Hazard No." HeaderStyle-Width="180px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblJhaId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHAID") %>' Visible="false"></telerik:RadLabel>
                            <asp:LinkButton ID="lnkJhaNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Form No." HeaderStyle-Width="180px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>' Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID") %>' Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblRevision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISION") %>' Visible="false"></telerik:RadLabel>
                            <asp:LinkButton ID="lnkFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Form Name">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" UniqueName="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>                                
                                <asp:ImageButton runat="server" ID="cmdAtt" ToolTip="Attachment" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
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
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
