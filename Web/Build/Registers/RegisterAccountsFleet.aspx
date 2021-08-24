<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterAccountsFleet.aspx.cs"
    Inherits="Registers_RegisterAccountsFleet" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Src="~/UserControls/UserControlTitle.ascx" TagPrefix="eluc" TagName="Title" %>
<%@ Register Src="~/UserControls/UserControlErrorMessage.ascx" TagPrefix="eluc" TagName="Error" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="Status"
    TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="Status1" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuGeneral" runat="server"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuAccountsFleetList" runat="server" OnTabStripCommand="MenuAccountsFleetList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAccountsFleet" Height="83%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                EnableViewState="true" OnNeedDataSource="gvAccountsFleet_NeedDataSource" OnSortCommand="gvAccountsFleet_SortCommand" >
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVESSELID">
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
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type" AllowSorting="true" SortExpression="">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Principal" AllowSorting="true" SortExpression="">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPRINCIPALNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag" AllowSorting="true" SortExpression="">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Administrator" AllowSorting="true" SortExpression="">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountAdmin" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACCOUNTADMINNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Accounts Executive" AllowSorting="true" SortExpression="">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountExec" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACCOUNTSEXECUTIVENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
