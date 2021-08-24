<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferJobs.aspx.cs"
    Inherits="DataTransferJobs" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Data Transfer Scheduled Jobs</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
                DecorationZoneID="divGrid" DecoratedControls="All" EnableRoundedCorners="true" />
            <table cellpadding="0">
                <tr>
                    <td>
                        <p>
                            <b>The status column will show SUCCESSFUL when the job is completed without errors.<br />
                                The status column will show FAILED when the job is completed with errors.
                                <br />
                                To report error on failed jobs, click on the link in the column 'Job Description'
                                take a screen shot of the error message<br />
                                and mail it to office.<br />
                            </b>
                        </p>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFolderList" runat="server" Height="98.5%"
                AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" CellSpacing="0"
                GridLines="None" OnNeedDataSource="gvFolderList_NeedDataSource" OnItemDataBound="gvFolderList_ItemDataBound"
                OnItemCommand="gvFolderList_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDID" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                    <HeaderStyle Width="102px" HorizontalAlign="Center" />
                    <CommandItemSettings ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true"
                        ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Scheduled Job ID" HeaderStyle-Width="134px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScheduledJobID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Description" HeaderStyle-Width="134px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblScheduledSQL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSQL") %>'
                                    CommandName="SHOWERROR">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Run On" HeaderStyle-Width="134px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastRunOn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTRUNON") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="134px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastRunOk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTRUNOK") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="134px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="REMOVE" ID="cmdImport" ToolTip="Remove">
                                <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="PROGRESS" ID="cmdProgress" ToolTip="Progress">
                                        <span class="icon"><i class="fas fa-redo-alt"></i></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
