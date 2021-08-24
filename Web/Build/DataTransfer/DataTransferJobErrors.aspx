<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferJobErrors.aspx.cs"
    Inherits="DataTransferJobErrors" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Data Synchronizer - Scheduled Jobs</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
            position: absolute; z-index: +1">
            <eluc:TabStrip ID="MenuJobs" runat="server" OnTabStripCommand="Jobs_TabStripCommand">
            </eluc:TabStrip>
            <div id="divGrid" style="position: relative; z-index: 0;">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvFolderList" runat="server" Height="98.5%"
                    AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" CellSpacing="0"
                    GridLines="None" OnNeedDataSource="gvFolderList_NeedDataSource" OnItemDataBound="gvFolderList_ItemDataBound"
                    OnItemCommand="gvFolderList_ItemCommand" AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                        <HeaderStyle Width="102px" HorizontalAlign="Center" />
                        <CommandItemSettings ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true"
                            ShowExportToPdfButton="false" />
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
                <telerik:RadGrid RenderMode="Lightweight" ID="gvTrace" runat="server" AllowCustomPaging="false"
                    AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" OnNeedDataSource="gvTrace_NeedDataSource"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                        <HeaderStyle Width="102px" HorizontalAlign="Center" />
                        <CommandItemSettings ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true"
                            ShowExportToPdfButton="false" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Message Log" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMsg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMSG") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Log Date/Time" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
