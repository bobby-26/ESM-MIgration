<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsUserIdentityLog.aspx.cs" Inherits="OptionsUserIdentityLog" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>


<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Identity Log</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmUserIdentity" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <%-- <eluc:Title runat="server" ID="ucTitle" Text="User Identity Log"  ShowMenu="false"/>--%>
            <eluc:TabStrip ID="MenuRemoteUser" runat="server" OnTabStripCommand="MenuRemoteUser_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table runat="server">
                <tr>
                    <td style="padding-right: 30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lblUID" runat="server" Text="UID"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 30px">
                        <telerik:RadTextBox ID="txtUID" runat="server" CssClass="gridinput_mandatory" Width="270px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuuserIdentityLog" runat="server" OnTabStripCommand="MenuuserIdentityLog_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvUseridentityLog" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvUseridentityLog_NeedDataSource" OnSorting="gvUseridentityLog_Sorting" OnItemCommand="gvUseridentityLog_ItemCommand"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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
                         <telerik:GridTemplateColumn HeaderText="UID" AllowSorting="true" SortExpression="FLDUID">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUID") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Mac Address" AllowSorting="true" SortExpression="FLDMACADDRESS">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMacAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACADDRESS") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IP Address" AllowSorting="true" SortExpression="FLDIPADDRESS">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIPAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIPADDRESS") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Machine">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMachine" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remote Address">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemoteAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMOTEADDR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Created Date">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

