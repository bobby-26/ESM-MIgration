<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardImportError.aspx.cs" Inherits="DashboardImportError" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Synchronizer - Scheduled Jobs</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuJobs" runat="server" OnTabStripCommand="Jobs_TabStripCommand"></eluc:TabStrip>
            <div id="divPage1" style="position: relative; z-index: +2">
                <telerik:RadGrid ID="gvFolderList" runat="server" AutoGenerateColumns="false" EnableViewState="false" CellSpacing="0" GridLines="None"
                    Height="50%" OnNeedDataSource="gvFolderList_NeedDataSource" Width="100%">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
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
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    To report error, copy the text below and send it by e-mail [To Copy click on text
                                    below and press Ctrl + A and then Ctrl + C. Paste it in email using Ctrl + V]
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDataTransferDateTime" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox TextMode="MultiLine" Rows="12" Columns="120" Width="100%" ID="lblDataTransferStartTime"
                                                    Font-Names="Tahoma, Arial" Font-Size="9pt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDERRORMESSAGE") %>'></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>

                    </MasterTableView>
                </telerik:RadGrid>
            </div>

            <div id="divPage2" style="position: relative; z-index: +1">
                <telerik:RadGrid ID="gvTrace" runat="server" AutoGenerateColumns="false" EnableViewState="false" CellSpacing="0" GridLines="None"
                    Height="40%" OnNeedDataSource="gvTrace_NeedDataSource" Width="100%">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
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
                            <telerik:GridTemplateColumn HeaderText="Message Log" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMsg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMSG") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Log Date/Time" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
