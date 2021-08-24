<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogEngineAttributeHistory.aspx.cs" Inherits="ElectricLogEngineAttributeHistory" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Revision History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resizediv() {
                var tbl = document.getElementById("tblComments");
                if (tbl != null) {
                    for (var i = 0; i < tbl.rows.length; i++) {
                        tbl.rows[i].cells[2].getElementsByTagName("div")[0].style.width = tbl.rows[i].cells[2].offsetWidth + "px";
                    }
                }
            }//script added for fixing Div width for the comments table
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resizediv();">
    <form id="frmIncidentCARExtensionReason" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid ID="gvExtensionReason" runat="server" AutoGenerateColumns="False" Font-Size="11px" Height="99%"
                AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false"
                Width="100%" CellPadding="3" OnItemCommand="gvExtensionReason_ItemCommand" OnItemDataBound="gvExtensionReason_ItemDataBound"
                ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvExtensionReason_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDREVISIONID">
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
                        <telerik:GridTemplateColumn HeaderText="Rev No">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONID") %>' Visible="false"></telerik:RadLabel>                                
                                <asp:LinkButton ID ="lnkOldRevNo" CommandName="REVNO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO") %>' ></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By">
                            <HeaderStyle Width="20%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcreatedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBYNAME") %>'></telerik:RadLabel>                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created Date">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>                                
                                <telerik:RadLabel ID="lblCreatedtDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Reason">
                            <HeaderStyle Width="30%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Published By">
                            <HeaderStyle Width="20%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPublishedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDBYNAME") %>'></telerik:RadLabel>              
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Published Date">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>                                
                                <telerik:RadLabel ID="lblPublishedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>                    
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>