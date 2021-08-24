<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRWAApprovalHistory.aspx.cs" Inherits="AccountsRWAApprovalHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Approval History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmApproval" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--<eluc:Title runat="server" ID="ucTitle" Text="History" ShowMenu="false" Visible="false" />
        <eluc:TabStrip ID="MenuHistory" runat="server" OnTabStripCommand="MenuHistory_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadLabel ID="lblaarh" runat="server" Text="Approve and Revoke History" Font-Bold="true"></telerik:RadLabel>--%>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvRevokeHistory" runat="server" AutoGenerateColumns="False" Width="100%"
            EnableHeaderContextMenu="true" GroupingEnabled="false"
            CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvRevokeHistory_NeedDataSource">
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
                    <telerik:GridTemplateColumn HeaderText="Name">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblUsername" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAPPROVEDBYNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Approved Date">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAPPROVEDDATE","{0:dd/MMM/yyyy HH:mm:ss}")%>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Status">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblremarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
     <%--       <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>--%>
        </telerik:RadGrid>
       
    </form>
</body>
</html>
